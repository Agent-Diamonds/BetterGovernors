using TaleWorlds.Core;
using TaleWorlds.Library; 
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using System.Collections.Generic;

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
            {
                InformationManager.DisplayMessage(new InformationMessage("Not Campaign Game Type"));
                return;
            }
            InformationManager.DisplayMessage(new InformationMessage("Campaign Game Type"));
            ((CampaignGameStarter)gameStarterObject).AddBehavior(new BetterGovernorsBehavior());
        }

        /// <summary>
        /// Called when the submodule loads. Lets player know the submodule has loaded properly.
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            var messageText = "Loaded Better Governors";
            var messageColor = Color.ConvertStringToColor("#03fcf8ff");
            InformationManager.DisplayMessage(new InformationMessage(messageText, messageColor));

        }

        /// <summary>
        /// Inner class that handles the core behavior of the BetterGovernors mod.
        /// </summary>
        internal class BetterGovernorsBehavior : CampaignBehaviorBase
        {
            /// <summary>
            /// Registers events for the campaign behavior.
            /// </summary>
            public override void RegisterEvents()
            {
                CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, HandleSettlementIssues);
            }

            /// <summary>
            /// Handles the settlement issues on a daily basis.
            /// </summary>
            private void HandleSettlementIssues()
            {
                var issueManager = Campaign.Current.IssueManager;
                if (issueManager == null)
                    return;

                foreach (var settlement in Clan.PlayerClan.Settlements)
                {
                    if (settlement.IsVillage || settlement.Town.Governor == null)
                        continue;

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
                var messageColor = Color.ConvertStringToColor("#03fcf8ff");
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
