using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;


namespace BetterGovernors
{
    /// <summary>
    /// Class to handle random skill selection for BetterGovernors mod.
    /// Selects an optional number of random skills with governing perks for xp gain based on probability distribution.
    /// </summary>
    internal class SkillSelector
    {
        // Enum representing the skills
        public enum GovernorSkills
        {
            Steward,
            Engineering,
            Medicine,
            Trade,
            Charm,
            Leadership,
            Roguery,
            Scouting,
            Tactics,
            Athletics,
            Riding,
            Throwing,
            Crossbow,
            Bow,
            Polearm,
            TwoHanded,
            OneHanded
        }

        //instance variables
        private readonly Dictionary<SkillObject, int> skillWeights;
        private Random random;


        /// <summary>
        /// Constructor, builds list of relevant governor skills and adds probability weights.
        /// </summary>
        public SkillSelector(List<SkillObject> skillList, bool uniformDistribution = false)
        {
            random = new Random();
            var governorSkillNames = Enum.GetNames(typeof(GovernorSkills));
            //Filter list of all skills down to only skills relevant to Governors
            var filteredSkills = skillList.Where(skill => governorSkillNames.Contains(skill.GetName().Value)).ToList();

            //populate dict of skills with weightings
            skillWeights = new Dictionary<SkillObject, int>();
            foreach (var skill in filteredSkills)
            {
                int weight = uniformDistribution ? 1 : DetermineWeight(skill.GetName().Value);
                skillWeights.Add(skill, weight);
            }
        }

        /// <summary>
        /// Determines the probability weighting for a skill based on the skill name.
        /// </summary>
        /// /// <param name="skillName">The name of the skill to retreive the weight for.</param>
        private int DetermineWeight(string skillName)
        {
            switch (skillName)
            {
                case nameof(GovernorSkills.Steward):
                    return 10;
                case nameof(GovernorSkills.Engineering):
                    return 8;
                case nameof(GovernorSkills.Medicine):
                    return 7;
                case nameof(GovernorSkills.Trade):
                    return 9;
                case nameof(GovernorSkills.Charm):
                    return 9;
                case nameof(GovernorSkills.Leadership):
                    return 10;
                case nameof(GovernorSkills.Roguery):
                    return 5;
                case nameof(GovernorSkills.Scouting):
                    return 2;
                case nameof(GovernorSkills.Tactics):
                    return 4;
                case nameof(GovernorSkills.Athletics):
                    return 3;
                case nameof(GovernorSkills.Riding):
                    return 3;
                case nameof(GovernorSkills.Throwing):
                    return 2;
                case nameof(GovernorSkills.Crossbow):
                    return 2;
                case nameof(GovernorSkills.Bow):
                    return 2;
                case nameof(GovernorSkills.Polearm):
                    return 2;
                case nameof(GovernorSkills.TwoHanded):
                    return 2;
                case nameof(GovernorSkills.OneHanded):
                    return 2;
                default:
                    throw new Exception("Cannot determine weight for non-governor skill");
            }
        }


        /// <summary>
        /// Gets a list of random skills relevant to governing, according to a probability distribution.
        /// The same skill can be selected multiple times (allows replacement).
        /// </summary>
        /// <param name="numberOfSkillsToSelect">The number of randomly selected skills to return.</param>
        public List<SkillObject> GetRandomSkills(int numberOfSkillsToSelect)
        {
            List<SkillObject> selectedSkills = new List<SkillObject>();

            // Total sum of all weights
            int totalWeight = skillWeights.Sum(sw => sw.Value);

            for (int i = 0; i < numberOfSkillsToSelect; i++)
            {
                // Random selection based on totaling of weights
                int randomNumber = random.Next(totalWeight);
                //Console.WriteLine("Random number: " + randomNumber.ToString());
                //Console.WriteLine("Total Weight: " + totalWeight.ToString());
                int cumulative = 0;
                foreach (var skill in skillWeights)
                {
                    cumulative += skill.Value;
                    if (randomNumber < cumulative)
                    {
                        //Console.WriteLine("Cumulative: " + cumulative.ToString());
                        //Console.WriteLine(skill.ToString());
                        selectedSkills.Add(skill.Key);
                        break; // Break out of the foreach loop once a skill is selected
                    }
                }
            }

            return selectedSkills;
        }
    }
}
