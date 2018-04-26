//---------------------------------------------
//File:   HighScoreTesting.cs
//Desc:   This file contains code to test the
//        capabilites of the HighScore.cs file.
//---------------------------------------------
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipBattlesModel
{
    // Contains four unique tests for testing the abilities of the HighScore and Score classes.
    // Also contains a helper method to reduce duplicate code.
    [TestClass]
    public class HighScoreTesting
    {
        // A HighScore instance that is used in the four tests.
        HighScore hsTest = new HighScore();

        // The aforementioned helper method. Reset the high-scores file and checks
        // to make sure that it got reset.
        private void ResetFile()
        {
            File.Delete("test.txt");
            hsTest.CheckHighScoresFile(true);
            Assert.IsTrue(File.Exists("test.txt"));
        }

        // Tests to see if a file with no scores in it can be loaded correctly.
        [TestMethod]
        public void TestLoadNoScores()
        {
            ResetFile();

            hsTest.LoadHighScores(true);
            Assert.IsTrue(hsTest.ScoresList.Count == 0);
        }

        // Tests to see if saving a high-score works correctly.
        [TestMethod]
        public void TestSaveHighScore()
        {
            ResetFile();

            hsTest.SaveHighScore("Kyle", 6975, true);
            Assert.IsTrue(hsTest.ScoresList[0].Name == "Kyle");
            Assert.IsTrue(hsTest.ScoresList[0].Points == 6975);
        }

        // Tests to see if saving high-scores and the sorting logic
        // that is built into the saving method works correctly.
        [TestMethod]
        public void TestSaveAndSortHighScores()
        {
            ResetFile();

            hsTest.SaveHighScore("Waluigi", 6000, true);
            hsTest.SaveHighScore("Geo Stelar", 1250, true);
            hsTest.SaveHighScore("Sans", 2500, true);
            hsTest.SaveHighScore("Wavedash", 3675, true);
            hsTest.SaveHighScore("Audacity", 500, true);
            hsTest.SaveHighScore("Dr Edgar George Zomboss", 925, true);

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

        // Tests to see if saving mutiple identical scores works correctly.
        // An score with an identical number of points should only be thrown out
        // if it ties with the lowest score in scoresList (see the HighScore class).
        [TestMethod]
        public void TestSaveSameScores()
        {
            ResetFile();

            hsTest.SaveHighScore("Wow", 1000, true);
            hsTest.SaveHighScore("Wow", 1000, true);
            hsTest.SaveHighScore("Wow", 1000, true);
            hsTest.SaveHighScore("Wow", 1000, true);
            hsTest.SaveHighScore("Wow", 1000, true);
            hsTest.SaveHighScore("Wow", 1000, true);

            Assert.IsTrue(hsTest.ScoresList.Count == 5);
            for (int i = 0; i <= 4; i++)
            {
                Assert.IsTrue(hsTest.ScoresList[i].Name == "Wow");
                Assert.IsTrue(hsTest.ScoresList[i].Points == 1000);
            }
        }
    }
}