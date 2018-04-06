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
        public int AIShipSpeed = 1;
        int PlayerSpeed = 2;

        public void LoadWorld()
        {
            // Later I will use functions of the 'level' varaible to make the 
            // set up more complicated.

            // Make sure that you set a bullet speed. 
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
            return new Direction() { Up = (rand.Next(3) - 1) , Right = (rand.Next(3) - 1) };
        }

        public Location MakeRandLocation()
        {
            return new Location() { X = rand.Next(World.Width), Y = rand.Next(World.Height) };
        }

    }

    public class HighScore
    {
        string filepath = "../../highscores.txt";
        string file = "highscores.txt";
        private List<string> scoresList = new List<string> { };
        // from https://stackoverflow.com/questions/3259583/how-to-get-files-in-a-relative-path-in-c-sharp
        // static string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Archive\";
        // static string filter = "*.txt";
        // string[] files = Directory.GetFiles(folder, filter);

        public List<string> ScoresList
        {
            get
            {
                return scoresList;
            }
        }

        // Saves a high-score to an output file (it first creates the output file if it does not exist)
        // If the maximum number of high-scores is reached, it deletes the lowest high-score
        // If the newest high-score
        public void SaveHighScore(string username, string highScore)
        {
            // File.Exists from https://stackoverflow.com/questions/38960/how-to-find-out-if-a-file-exists-in-c-sharp-net
            if (!File.Exists(file))
            {
                File.Create(filepath);
            }
            using (FileStream fs = File.Open(filepath, FileMode.Open))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(username + " " + highScore);
                }
            }
        }

        // Load a list of high-scores from said output file
        public void LoadHighScores()
        {
            if (!File.Exists(file))
            {
                scoresList.Add("There are currently no high-scores. Play a level to get the first high-score!");
                return;
            }
            using (FileStream fs = File.Open(filepath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    for (int i = 0; i == 10; i++)
                    {
                        string[] tempString = sr.ReadLine().Split(' ');
                        scoresList.Add(tempString[0]);
                        scoresList.Add(tempString[1]);
                    }
                }
            }
        }
    }
}
