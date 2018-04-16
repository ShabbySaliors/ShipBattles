using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipBattlesModel
{
    [TestClass]
    public class HighScoreTesting
    {
        HighScore hsTest = new HighScore();

        [TestMethod]
        public void TestLoadNoScores()
        {
            File.Delete(hsTest.Filename);
            hsTest.CheckHighScoresFile();
            Assert.IsTrue(File.Exists(hsTest.Filename));

            hsTest.LoadHighScores(false);
            Assert.IsTrue(hsTest.ScoresList.Count == 0);
        }

        [TestMethod]
        public void TestSaveHighScore()
        {
            File.Delete(hsTest.Filename);
            hsTest.CheckHighScoresFile();
            Assert.IsTrue(File.Exists(hsTest.Filename));

            hsTest.SaveHighScore("Bob", 5000);
            Assert.IsTrue(hsTest.ScoresList[0].Name == "Bob");
            Assert.IsTrue(hsTest.ScoresList[0].Points == 5000);
        }

        [TestMethod]
        public void TestLoadAllScores()
        {
            hsTest.LoadHighScores(true);
            Assert.IsTrue(hsTest.ScoresList.Count == 5);
            Assert.IsTrue(hsTest.ScoresList[0].Name == "Kyle");
            Assert.IsTrue(hsTest.ScoresList[0].Points == 6975);
            Assert.IsTrue(hsTest.ScoresList[1].Name == "Jeff");
            Assert.IsTrue(hsTest.ScoresList[1].Points == 4000);
            Assert.IsTrue(hsTest.ScoresList[2].Name == "James");
            Assert.IsTrue(hsTest.ScoresList[2].Points == 3000);
            Assert.IsTrue(hsTest.ScoresList[3].Name == "Desmond");
            Assert.IsTrue(hsTest.ScoresList[3].Points == 2525);
            Assert.IsTrue(hsTest.ScoresList[4].Name == "Alex");
            Assert.IsTrue(hsTest.ScoresList[4].Points == 1000);
        }

        [TestMethod]
        public void TestSaveAndSortHighScores()
        {
            File.Delete(hsTest.Filename);
            hsTest.CheckHighScoresFile();
            Assert.IsTrue(File.Exists(hsTest.Filename));

            hsTest.SaveHighScore("Waluigi", 6000);
            hsTest.SaveHighScore("Geo Stelar", 1250);
            hsTest.SaveHighScore("Sans", 2500);
            hsTest.SaveHighScore("Wavedash", 3675);
            hsTest.SaveHighScore("Audacity", 500);
            hsTest.SaveHighScore("Dr Edgar George Zomboss", 925);

            Assert.IsTrue(hsTest.ScoresList.Count == 5);
            Assert.IsTrue(hsTest.ScoresList[0].Name == "Waluigi");
            Assert.IsTrue(hsTest.ScoresList[0].Points == 6000);
            Assert.IsTrue(hsTest.ScoresList[1].Name == "Wavedash");
            Assert.IsTrue(hsTest.ScoresList[1].Points == 3675);
            Assert.IsTrue(hsTest.ScoresList[2].Name == "Sans");
            Assert.IsTrue(hsTest.ScoresList[2].Points == 2500);
            Assert.IsTrue(hsTest.ScoresList[3].Name == "Geo_Stelar");
            Assert.IsTrue(hsTest.ScoresList[3].Points == 1250);
            Assert.IsTrue(hsTest.ScoresList[4].Name == "Dr_Edgar_George_Zomboss");
            Assert.IsTrue(hsTest.ScoresList[4].Points == 925);
        }
    }
}
