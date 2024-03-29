﻿using TaleWorlds.Core;
using TaleWorlds.Library; 
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using TaleWorlds.ObjectSystem;
using TaleWorlds.CampaignSystem.Extensions;

namespace BetterGovernors
{
    /// <summary>
    /// Main class for the BetterGovernors mod.
    /// Handles initialization and configuration of the mod.
    /// </summary>
    public class BetterGovernors : MBSubModuleBase
    {

        /// <summary>
        /// Called when the game starts. Adds the BetterGovernorsBehavior to the game if it's a campaign.
        /// </summary>
        /// <param name="game">The game object.</param>
        /// <param name="gameStarterObject">The game starter object.</param>
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
                return;
            InformationManager.DisplayMessage(new InformationMessage("Loaded Better Governors"));
            ((CampaignGameStarter)gameStarterObject).AddBehavior(new BetterGovernorsBehavior());

        }

        /// <summary>
        /// Inner class that handles the core behavior of the BetterGovernors mod.
        /// </summary>
        internal class BetterGovernorsBehavior : CampaignBehaviorBase
        {
            //instance variables
            private SkillSelector skillSelector;


            /// <summary>
            /// Constructor, prebuilds random skills selector.
            /// </summary>
            public BetterGovernorsBehavior() {
                List<SkillObject> skillList = (List<SkillObject>)MBObjectManager.Instance.GetObjectTypeList<SkillObject>();
                skillSelector = new SkillSelector(skillList);
            }
            /// <summary>
            /// Registers events for the campaign behavior.
            /// </summary>
            public override void RegisterEvents()
            {
                CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, this.ProcessDailyGovernorActions);
            }


            /// <summary>
            /// Allots Governor's xp for Governing. Called when resolving issues to avoid iterating towns twice.
            /// </summary>
            /// /// <param name="governor">The governor to give xp to.</param>
            private void GiveGovernorExperience(Hero governor)
            {
                int numberOfSkillsToLevel = 3; //constant for now, will be variable later when options are added
                float xpToGive = 80; //constant for now, will be variable later when options are added
                List <SkillObject> skillsToLevel = this.skillSelector.GetRandomSkills(numberOfSkillsToLevel);
                //Dict to hold cumulative xp from duplicate skill selections
                Dictionary<string, float> skillXpMap = new Dictionary<string, float>();
                //Dict to map from skill name to in game SkillObject
                Dictionary<string, SkillObject> nameToSkillMap = new Dictionary<string, SkillObject>();

                //Accumulate xp for each skill to level
                foreach (SkillObject skill in skillsToLevel)
                {
                    string skillName = skill.GetName().Value;
                    if (skillXpMap.ContainsKey(skillName))
                    {
                        skillXpMap[skillName] += xpToGive;
                    }
                    else
                    {
                        skillXpMap[skillName] = xpToGive;
                        nameToSkillMap[skillName] = skill;
                    }
                    
                }

                // Add XP and display messages about cumulative xp gain per skill
                foreach (var skillXp in skillXpMap)
                {
                    string skillName = skillXp.Key;
                    SkillObject skill = nameToSkillMap[skillName];

                    string messageText = $"{governor.Name} gained {skillXp.Value} xp in {skillName}.";
                    InformationManager.DisplayMessage(new InformationMessage(messageText));
                    governor.AddSkillXp(skill, skillXp.Value); // Add cumulative XP to the skill
                }
            }

            /// <summary>
            /// Handles the settlement issues on a daily basis.
            /// </summary>
            private void ProcessDailyGovernorActions()
            {
                var issueManager = Campaign.Current.IssueManager;
                if (issueManager == null)
                    return;

                //iterate through all the player settlements
                foreach (var settlement in Clan.PlayerClan.Settlements)
                {
                    //Select Towns (not villages) that have a govenor. Villages can't have governor's, so we check towns only
                    if (settlement.IsVillage || settlement.Town.Governor == null)
                        continue;

                    //Give the governor XP, resolve any issues in the Town and then its bound villages
                    GiveGovernorExperience(settlement.Town.Governor);
                    ResolveIssuesInSettlement(settlement, issueManager.Issues);
                    ResolveIssuesInBoundVillages(settlement, issueManager.Issues);
                }
            }

            /// <summary>
            /// Resolves issues in a given settlement.
            /// </summary>
            /// <param name="settlement">The settlement to check issues in.</param>
            /// <param name="issues">The dictionary of issues to resolve.</param>
            private void ResolveIssuesInSettlement(Settlement settlement, MBReadOnlyDictionary<Hero, IssueBase> issues)
            {
                bool messageDisplayed = false;
                foreach (var notable in settlement.Notables)
                {
                    foreach (var issue in issues.Values)
                    {
                        if (IssueBelongsToNotable(issue, notable))
                        {
                            issue.CompleteIssueWithCancel(null);
                            if (!messageDisplayed)
                            {
                                DisplayGovernorActionMessage(settlement.Town.Governor, settlement);
                                messageDisplayed = true;
                                break;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Resolves issues in bound villages of a given settlement.
            /// </summary>
            /// <param name="settlement">The settlement whose bound villages are checked.</param>
            /// <param name="issues">The dictionary of issues to resolve.</param>
            private void ResolveIssuesInBoundVillages(Settlement settlement, MBReadOnlyDictionary<Hero, IssueBase> issues)
            {
                foreach (var village in settlement.BoundVillages)
                {
                    bool messageDisplayed = false;
                    foreach (var notable in village.Settlement.Notables)
                    {
                        foreach (var issue in issues.Values)
                        {
                            if (IssueBelongsToNotable(issue, notable))
                            {
                                issue.CompleteIssueWithCancel(null);
                                if (!messageDisplayed)
                                {
                                    DisplayGovernorActionMessage(settlement.Town.Governor, village.Settlement);
                                    messageDisplayed = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Checks if an issue belongs to a given notable.
            /// </summary>
            /// <param name="issue">The issue to check.</param>
            /// <param name="notable">The notable to compare against.</param>
            /// <returns>True if the issue belongs to the notable; otherwise false.</returns>
            private bool IssueBelongsToNotable(IssueBase issue, Hero notable)
            {
                return issue != null && issue.IssueOwner != null && issue.IssueOwner == notable;
            }

            /// <summary>
            /// Displays a message indicating that the governor has resolved issues in a settlement.
            /// </summary>
            /// <param name="governor">The governor who resolved the issues.</param>
            /// <param name="settlement">The settlement where issues were resolved.</param>
            private void DisplayGovernorActionMessage(Hero governor, Settlement settlement)
            {
                string messageText = $"{governor.Name} settled issues in {settlement.Name}.";
                var messageColor = Color.ConvertStringToColor("#a318c9ff");
                InformationManager.DisplayMessage(new InformationMessage(messageText, messageColor));
            }

            /// <summary>
            /// Syncs data for the campaign behavior.
            /// </summary>
            /// <param name="dataStore">The data store for syncing data.</param>
            public override void SyncData(IDataStore dataStore)
            {
                // Implementation if needed
            }
        }
    }
}
