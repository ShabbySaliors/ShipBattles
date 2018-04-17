using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShipBattlesModel
{
    public class HighScore
    {
        string filename = "highscores.txt";
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
        public void CheckHighScoresFile()
        {
            // from https://msdn.microsoft.com/en-us/library/system.io.file.exists%28v=vs.110%29.aspx
            if (!File.Exists(filename))
            {
                // from https://stackoverflow.com/questions/5156254/closing-a-file-after-file-create
                var temp = File.Create(filename);
                temp.Close();
            }
        }

        // Saves a high-score to an output file (it first creates the output file if it does not exist)
        // If the maximum number of high-scores is reached, it deletes the lowest high-score
        // (or the newest high-score if it ties with the lowest)
        public void SaveHighScore(string username, int highScore)
        {
            string nameToSave = "";
            int i = 0;
            bool isUniqueScore = false;
            FileStream fs1 = File.Open(filename, FileMode.Open);
            StreamWriter sw1 = new StreamWriter(fs1);

            foreach (char c in username)
            {
                if (c == ' ') nameToSave += '_';
                else nameToSave += c;
            }

            Score score = new Score(nameToSave, highScore);
            if (scoresList.Count == 0) scoresList.Add(score);
            else
            {
                foreach (Score s in scoresList)
                {
                    if (score.Points == s.Points) isUniqueScore = true;
                    if (isUniqueScore == false)
                    {
                        scoresList.Add(score);
                        break;
                    }
                }
            }
            scoresList.Sort();
            if (scoresList.Count == 6)
            {
                scoresList.RemoveAt(0);
            }
            scoresList.Reverse();

            sw1.Close();
            fs1.Close();
            File.Delete(filename);
            CheckHighScoresFile();
            FileStream fs2 = File.Open(filename, FileMode.Open);
            StreamWriter sw2 = new StreamWriter(fs2);
            foreach (Score s in scoresList)
            {
                if (i == (scoresList.Count - 1)) sw2.Write(s.Name + " " + s.Points.ToString());
                else sw2.WriteLine(s.Name + " " + s.Points.ToString());
                i++;
            }
            sw2.Close();
            fs2.Close();
        }

        // Load a list of high-scores from said output file
        // testMode will trigger an alternate filename for unit tests that need it
        public void LoadHighScores(bool testMode)
        {
            string nameToLoad = "";
            // here is the trigger
            if (testMode) filename = "test.txt";
            if (File.Exists(filename))
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        for (int i = 0; i <= 4; i++)
                        {
                            string tempString1;
                            if ((tempString1 = sr.ReadLine()) != null)
                            {
                                string[] tempString2 = tempString1.Split(' ');
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
            // reset filename regardless of trigger activation
            filename = "highscores.txt";
        }
    }

    // taken and changed from https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort?view=netframework-4.7.1#System_Collections_Generic_List_1_Sort_System_Comparison__0
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