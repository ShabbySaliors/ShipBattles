using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ShipBattlesModel
{
    public class Controller
    {
        public string Username;
        public GameWorld World { get; set; }
        public int level = 0;
        Random rand = new Random();
        public double AIShipSpeed = 1;
        double PlayerSpeed = 2;

        public void LoadWorld()
        {
            // Later I will use functions of the 'level' varaible to make the 
            // set up more complicated.
            World.Width = 300;
            World.Height = 300;
            for (int i = 0; i < 5; i++)
            {
                World.Objects.Add(new AIShip() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < 4; i++)
            {
                World.Objects.Add(new Base() { Loc = MakeRandLocation() });
            }
            for (int i = 0; i < 4; i++)
            {
                World.Objects.Add(new Asteroid() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < 3; i++)
            {
                World.Objects.Add(new RepairKit() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }

            World.PlayerShip = new PlayerShip() { Loc = MakeRandLocation(), Speed = PlayerSpeed };
            World.Objects.Add(World.PlayerShip);
        }

        public void Save()
        {
            Console.WriteLine("Level:");
            Console.WriteLine(level);
            Console.WriteLine("Username:");
            Console.WriteLine(Username);
            Console.WriteLine("AIShipspeed:");
            Console.WriteLine(AIShipSpeed);
            Console.WriteLine("PlayerSpeed");
            Console.WriteLine(PlayerSpeed);
            // etc
        }

        public void Load()
        {
            // Some code.
        }

        public Direction MakeRandDirection()
        {
            return new Direction() { Up = (rand.NextDouble() - 0.5) * 2, Right = (rand.NextDouble() - 0.5) };
        }

        public Location MakeRandLocation()
        {
            return new Location() { X = rand.Next(World.Width), Y = rand.Next(World.Height) };
        }

    }

    public class HighScore
    {
        string file = "highscores.txt";
        private List<string> scoresList = new List<string> { };

        public List<string> ScoresList
        {
            get
            {
                return scoresList;
            }
        }

        // Checks if the high-scores file exists
        // If it does not, it makes a new high-scores file
        // After the check, it initializes scoresList with 10 empty strings
        // scoresList will be changed into an array later
        public void CheckHighScoresFile()
        {
            // from https://msdn.microsoft.com/en-us/library/system.io.file.exists%28v=vs.110%29.aspx
            if (!File.Exists(file))
            {
                // from https://stackoverflow.com/questions/5156254/closing-a-file-after-file-create
                var temp = File.Create(file);
                temp.Close();
            }
            for (int i = 0; i <= 9; i++)
            {
                scoresList.Add("");
            }
        }

        // Saves a high-score to an output file (it first creates the output file if it does not exist)
        // If the maximum number of high-scores is reached, it deletes the lowest high-score
        // If the newest high-score
        public void SaveHighScore(string username, string highScore)
        {           
            using (FileStream fs = File.Open(file, FileMode.Open))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    // test input
                    sw.WriteLine(username + " " + highScore);
                    scoresList[0] = username;
                    scoresList[1] = highScore;
                }
            }
        }

        // Load a list of high-scores from said output file
        public void LoadHighScores()
        {
            if (File.Exists(file))
            {
                using (FileStream fs = File.Open(file, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        for (int i = 0; i <= 19; i+=2)
                        {
                            string tempString1;
                            if ((tempString1 = sr.ReadLine()) != null)
                            {
                                string[] tempString2 = tempString1.Split(' ');
                                scoresList[i] = tempString2[0];
                                scoresList[i + 1] = tempString2[1];
                            }
                        }
                    }
                }
            }
        }
    }
}
