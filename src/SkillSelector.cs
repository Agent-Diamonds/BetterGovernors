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
        public SkillSelector()
        {
            random = new Random();
            skillWeights = new Dictionary<GovernorSkills, int>
            {
                { GovernorSkills.Steward, 1 },
                { GovernorSkills.Engineering,1 },
                { GovernorSkills.Medicine, 1 },
                { GovernorSkills.Trade, 1 },
                { GovernorSkills.Charm, 1 },
                { GovernorSkills.Leadership, 1 },
                { GovernorSkills.Roguery, 1 },
                { GovernorSkills.Scouting, 1 },
                { GovernorSkills.Tactics, 1 },
                { GovernorSkills.Athletics, 1 },
                { GovernorSkills.Riding, 1 },
                { GovernorSkills.Throwing, 1 },
                { GovernorSkills.Crossbow, 1 },
                { GovernorSkills.Bow, 1 },
                { GovernorSkills.Polearm, 1 },
                { GovernorSkills.TwoHanded, 1 },
                { GovernorSkills.OneHanded, 1 }
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
                Console.WriteLine("Random number: " + randomNumber.ToString());
                Console.WriteLine("Total Weight: " + totalWeight.ToString());
                int cumulative = 0;
                foreach (var skill in skillWeights)
                {
                    cumulative += skill.Value;
                    if (randomNumber < cumulative)
                    {
                        Console.WriteLine("Cumulative: " + cumulative.ToString());
                        Console.WriteLine(skill.ToString());
                        selectedSkills.Add(skill.Key);
                        break; // Break out of the foreach loop once a skill is selected
                    }
                }
            }

            return selectedSkills;
        }
    }
}
