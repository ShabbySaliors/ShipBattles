using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            for( int i = 0; i < 5; i++)
            {
                World.Objects.Add(new AIShip() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed});
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

        public class HighScore
        {

            // Saves a highscore to an output file (and creates the output file if it does not exist)
            // If the maximum number of highscores is reached, it deletes the lowest highscore (unless the newest highscore is the lowest)
            public void SaveHighScore(string username, int highScore)
            {
                using (FileStream fs = File.Open(highscores.txt, FileMode.Open))
                {
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        sr.WriteLine(username + " " + highScore.ToString());
                    }
                }
            }

            // Load a list of highscores from said output file
            public void LoadHighScores()
            {
                // Some more code.
            }
        }
    }

    public class HighScore
    {

        // Saves a highscore to an output file (and creates the output file if it does not exist)
        // If the maximum number of highscores is reached, it deletes the lowest highscore (unless the newest highscore is the lowest)
        public void SaveHighScore(string username, int highScore)
        {
            using (FileStream fs = File.Open(highscores.txt, FileMode.Open))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(username + " " + highScore.ToString());
                }
            }
        }

        // Load a list of highscores from said output file
        public void LoadHighScores()
        {
            // Some more code.
        }
    }
}
