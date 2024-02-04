using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterGovernors;
using System;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class SkillSelectorTest
    {
        [TestMethod]
        public void TestSkillSelectorNumber()
        {
            var skillSelector = new SkillSelector();
            List<SkillSelector.GovernorSkills> skills = skillSelector.GetRandomSkills(5);
            Assert.AreEqual(skills.Count, 5);
            Console.WriteLine(string.Join(", ", skills));
        }

        [TestMethod]
        public void ManuallyVerifyUniformDistribution()
        {
            var skillSelector = new SkillSelector(true);
            var skillCounts = new Dictionary<SkillSelector.GovernorSkills, int>();

            // Initialize the count for each skill to 0
            foreach (SkillSelector.GovernorSkills skill in Enum.GetValues(typeof(SkillSelector.GovernorSkills)))
            {
                skillCounts[skill] = 0;
            }

            int numberOfTrials = 100000; // Large number of trials
            int numberOfSkillsToSelect = 1; // Number of skills to select in each trial

            // Conduct trials
            for (int i = 0; i < numberOfTrials; i++)
            {
                var selectedSkills = skillSelector.GetRandomSkills(numberOfSkillsToSelect);
                foreach (var skill in selectedSkills)
                {
                    skillCounts[skill]++;
                }
            }

            //Write total skill count selections
            Console.WriteLine(string.Join(", ", skillCounts));
            // Visualization
            Console.WriteLine("Skill Selection Frequency Visualization:");
            foreach (var skill in skillCounts)
            {
                Console.WriteLine($"{skill.Key.ToString(),-15}: {new string('#', skill.Value / 100)}"); // Scaling the visualization
            }
        }

        [TestMethod]
        public void ManuallyVerifyNonUniformDistribution()
        {
            var skillSelector = new SkillSelector();
            var skillCounts = new Dictionary<SkillSelector.GovernorSkills, int>();

            // Initialize the count for each skill to 0
            foreach (SkillSelector.GovernorSkills skill in Enum.GetValues(typeof(SkillSelector.GovernorSkills)))
            {
                skillCounts[skill] = 0;
            }

            int numberOfTrials = 100000; // Large number of trials
            int numberOfSkillsToSelect = 1; // Number of skills to select in each trial

            // Conduct trials
            for (int i = 0; i < numberOfTrials; i++)
            {
                var selectedSkills = skillSelector.GetRandomSkills(numberOfSkillsToSelect);
                foreach (var skill in selectedSkills)
                {
                    skillCounts[skill]++;
                }
            }

            //Write total skill count selections
            Console.WriteLine(string.Join(", ", skillCounts));
            // Visualization
            Console.WriteLine("Skill Selection Frequency Visualization:");
            foreach (var skill in skillCounts)
            {
                Console.WriteLine($"{skill.Key.ToString(),-15}: {new string('#', skill.Value / 100)}"); // Scaling the visualization
            }
        }

    }
}
