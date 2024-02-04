using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Extensions;


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
        private readonly Dictionary<GovernorSkills, int> skillWeights;
        private Random random;


        /// <summary>
        /// Constructor, builds list of relevant governor skills and adds probability weights.
        /// </summary>
        public SkillSelector(bool uniformDistribution = false)
        {
            random = new Random();
            skillWeights = new Dictionary<GovernorSkills, int>
            {
                { GovernorSkills.Steward, uniformDistribution ? 1 : 10 },
                { GovernorSkills.Engineering,uniformDistribution ? 1 : 8 },
                { GovernorSkills.Medicine, uniformDistribution ? 1 : 7 },
                { GovernorSkills.Trade, uniformDistribution ? 1 : 9 },
                { GovernorSkills.Charm, uniformDistribution ? 1 : 9 },
                { GovernorSkills.Leadership, uniformDistribution ? 1 : 10 },
                { GovernorSkills.Roguery,uniformDistribution ? 1 : 5 },
                { GovernorSkills.Scouting, uniformDistribution ? 1 : 2 },
                { GovernorSkills.Tactics, uniformDistribution ? 1 : 4 },
                { GovernorSkills.Athletics,uniformDistribution ? 1 : 3 },
                { GovernorSkills.Riding, uniformDistribution ? 1 : 3 },
                { GovernorSkills.Throwing, uniformDistribution ? 1 : 2 },
                { GovernorSkills.Crossbow, uniformDistribution ? 1 : 2 },
                { GovernorSkills.Bow, uniformDistribution ? 1 : 2 },
                { GovernorSkills.Polearm, uniformDistribution ? 1 : 2 },
                { GovernorSkills.TwoHanded, uniformDistribution ? 1 : 2 },
                { GovernorSkills.OneHanded, uniformDistribution ? 1 : 2 }
            };
        }

        /// <summary>
        /// Gets a list of random skills relevant to governing, according to a probability distribution.
        /// The same skill can be selected multiple times (allows replacement).
        /// </summary>
        /// <param name="numberOfSkillsToSelect">The number of randomly selected skills to return.</param>
        public List<GovernorSkills> GetRandomSkills(int numberOfSkillsToSelect)
        {
            List<GovernorSkills> selectedSkills = new List<GovernorSkills>();

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
