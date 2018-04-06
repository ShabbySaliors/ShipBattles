using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Currently does not work.
namespace ShipBattlesModel
{
    [TestClass]
    public class HighScoreTesting
    {
        string file = "highscores.txt";
        private List<string> scoresList = new List<string> { };
        HighScore hsTest = new HighScore();

        [TestMethod]
        public void TestLoadNoScores()
        {
            var temp = File.Create(file);
            temp.Close();
            hsTest.CheckHighScoresFile();
            hsTest.LoadHighScores();
            Assert.IsTrue(hsTest.ScoresList[0] == "");
        }

        [TestMethod]
        public void TestSaveHighScore()
        {
            var temp = File.Create(file);
            temp.Close();
            hsTest.CheckHighScoresFile();
            hsTest.SaveHighScore("Bob", "5000");
            Assert.IsTrue(hsTest.ScoresList[0] == "Bob");
            Assert.IsTrue(hsTest.ScoresList[1] == "5000");
        }
    }
}
