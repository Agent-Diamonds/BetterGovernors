using System;
using System.Collections.Generic;
using System.Linq;
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


        /// <summary>
        /// Constructor, builds list of relevant governor skills and adds probability weights.
        /// </summary>
        public SkillSelector()
        {
            skillWeights = new Dictionary<GovernorSkills, int>
            {
                { GovernorSkills.Steward, 10 },
                { GovernorSkills.Engineering, 5 },
                { GovernorSkills.Medicine, 5 },
                { GovernorSkills.Trade, 10 },
                { GovernorSkills.Charm, 5 },
                { GovernorSkills.Leadership, 10 },
                { GovernorSkills.Roguery, 5 },
                { GovernorSkills.Scouting, 5 },
                { GovernorSkills.Tactics, 5 },
                { GovernorSkills.Athletics, 3 },
                { GovernorSkills.Riding, 3 },
                { GovernorSkills.Throwing, 2 },
                { GovernorSkills.Crossbow, 2 },
                { GovernorSkills.Bow, 2 },
                { GovernorSkills.Polearm, 2 },
                { GovernorSkills.TwoHanded, 2 },
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
            var random = new Random();

            // Total sum of all weights
            int totalWeight = skillWeights.Sum(sw => sw.Value);

            for (int i = 0; i < numberOfSkillsToSelect; i++)
            {
                // Random selection based on totaling of weights
                int randomNumber = random.Next(totalWeight);
                int cumulative = 0;
                foreach (var skill in skillWeights)
                {
                    cumulative += skill.Value;
                    if (randomNumber < cumulative)
                    {
                        selectedSkills.Add(skill.Key);
                        break; // Break out of the foreach loop once a skill is selected
                    }
                }
            }

            return selectedSkills;
        }
    }
}
