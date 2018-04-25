//-----------------------------------------
//File:   HighScore.cs
//Desc:   This file contains code to manage
//        the high-scores in ShipBattles.
//-----------------------------------------
using System;
using System.Collections.Generic;
using System.IO;

namespace ShipBattlesModel
{
    // Manages checking for a high-scores file (which is seperate from the save file),
    // saving, and loading high-scores.
    public class HighScore
    {
        // testMode will trigger an alternate filename for unit tests
        private string filename = "highscores.txt";
        private List<Score> scoresList = new List<Score> { };
        public string Filename
        {
            get
            {
                return filename;
            }
        }
        public List<Score> ScoresList
        {
            get
            {
                return scoresList;
            }
        }

        // Checks if the high-scores file exists
        // If it does not, it makes a new high-scores file
        public void CheckHighScoresFile(bool testMode)
        {
            if (testMode) filename = "test.txt";
            // from https://msdn.microsoft.com/en-us/library/system.io.file.exists%28v=vs.110%29.aspx
            if (!File.Exists(filename))
            {
                // from https://stackoverflow.com/questions/5156254/closing-a-file-after-file-create
                var temp = File.Create(filename);
                temp.Close();
            }
            filename = "highscores.txt";
        }

        // Saves a high-score to an output file (it first creates the output file if it does not exist)
        // If the maximum number of high-scores is reached, it deletes the lowest high-score
        // (or the newest high-score if it ties with the lowest)
        public async void SaveHighScore(string username, int highScore, bool testMode)
        {
            string nameToSave = "";
            int i = 0;
            bool isUniqueScore = true;
            if (testMode) filename = "test.txt";

            foreach (char c in username)
            {
                if (c == ' ') nameToSave += '_';
                else nameToSave += c;
            }

            Score score = new Score(nameToSave, highScore);
            if (scoresList.Count <= 4) scoresList.Add(score);
            else
            {
                foreach (Score s in scoresList)
                {
                    if (score.Points == s.Points) isUniqueScore = false;
                    if (isUniqueScore)
                    {
                        scoresList.Add(score);
                        break;
                    }
                }
            }
            scoresList.Sort();
            if (scoresList.Count == 6) scoresList.RemoveAt(0);
            scoresList.Reverse();

            File.Delete(filename);
            CheckHighScoresFile(testMode);
            FileStream fs = File.Open(filename, FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);
            foreach (Score s in scoresList)
            {
                if (i == (scoresList.Count - 1)) await sw.WriteAsync(s.Name + " " + s.Points.ToString());
                else await sw.WriteLineAsync(s.Name + " " + s.Points.ToString());
                i++;
            }
            sw.Close();
            fs.Close();
            filename = "highscores.txt";
        }

        // Load a list of high-scores from said output file
        public async void LoadHighScores(bool testMode)
        {
            if (testMode) filename = "test.txt";
            if (scoresList.Count > 0) ScoresList.Clear();
            if (File.Exists(filename))
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        for (int i = 0; i <= 4; i++)
                        {
                            string tempString1;
                            if ((tempString1 = await sr.ReadLineAsync()) != null)
                            {
                                string[] tempString2 = tempString1.Split(' ');
                                string nameToLoad = "";
                                foreach (char c in tempString2[0])
                                {
                                    if (c == '_') nameToLoad += ' ';
                                    else nameToLoad += c;
                                }
                                scoresList.Add(new Score(nameToLoad, Convert.ToInt32(tempString2[1])));
                                nameToLoad = "";
                            }
                        }
                        if (scoresList.Count != 0)
                        {
                            scoresList.Sort();
                            scoresList.Reverse();
                        }
                    }
                }
            }
            filename = "highscores.txt";
        }
    }

    // A convenient class for sorting and managing high-scores.
    // Taken and changed from https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort?view=netframework-4.7.1#System_Collections_Generic_List_1_Sort_System_Comparison__0
    // (it is the first big code example)
    public class Score : IComparable<Score>
    {
        public string Name { get; set; }

        public int Points { get; set; }

        public Score(string name, int points)
        {
            Name = name;
            Points = points;
        }

        public int CompareTo(Score compareScore)
        {
            if (compareScore == null) return 1;
            else return this.Points.CompareTo(compareScore.Points);
        }
    }
}