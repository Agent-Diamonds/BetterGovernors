using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterGovernors;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace Test
{
    [TestClass]
    public class SkillSelectorTest
    {


        [TestMethod]
        public void TestSkillSelectorNumber()
        {
            List<SkillObject> allSkills = CreateFakeSkillList();
            var skillSelector = new SkillSelector(allSkills);
            List<SkillObject> skills = skillSelector.GetRandomSkills(5);

            Assert.AreEqual(skills.Count, 5);
        }

        [TestMethod]
        public void ManuallyVerifyUniformDistribution()
        {
            List<SkillObject> allSkills = CreateFakeSkillList();
            var skillSelector = new SkillSelector(allSkills, true);
            var skillCounts = new Dictionary<string, int>();

            // Initialize the count for each skill to 0
            foreach (SkillObject skill in allSkills)
            {
                skillCounts[skill.GetName().Value] = 0;
            }

            int numberOfTrials = 100000; // Large number of trials
            int numberOfSkillsToSelect = 1; // Number of skills to select in each trial

            // Conduct trials
            for (int i = 0; i < numberOfTrials; i++)
            {
                var selectedSkills = skillSelector.GetRandomSkills(numberOfSkillsToSelect);
                foreach (var skill in selectedSkills)
                {
                    skillCounts[skill.GetName().Value]++;
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
            List<SkillObject> allSkills = CreateFakeSkillList();
            var skillSelector = new SkillSelector(allSkills, false);
            var skillCounts = new Dictionary<string, int>();

            // Initialize the count for each skill to 0
            foreach (SkillObject skill in allSkills)
            {
                skillCounts[skill.GetName().Value] = 0;
            }

            int numberOfTrials = 100000; // Large number of trials
            int numberOfSkillsToSelect = 1; // Number of skills to select in each trial

            // Conduct trials
            for (int i = 0; i < numberOfTrials; i++)
            {
                var selectedSkills = skillSelector.GetRandomSkills(numberOfSkillsToSelect);
                foreach (var skill in selectedSkills)
                {
                    skillCounts[skill.GetName().Value]++;
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


        public static List<SkillObject> CreateFakeSkillList()
        {
            return new List<SkillObject>
            {
                new SkillObject("OneHanded"),
                new SkillObject("TwoHanded"),
                new SkillObject("Polearm"),
                new SkillObject("Bow"),
                new SkillObject("Crossbow"),
                new SkillObject("Throwing"),
                new SkillObject("Riding"),
                new SkillObject("Athletics"),
                new SkillObject("Smithing"),
                new SkillObject("Scouting"),
                new SkillObject("Tactics"),
                new SkillObject("Roguery"),
                new SkillObject("Charm"),
                new SkillObject("Leadership"),
                new SkillObject("Trade"),
                new SkillObject("Steward"),
                new SkillObject("Medicine"),
                new SkillObject("Engineering")
            };

        }
    }
}
