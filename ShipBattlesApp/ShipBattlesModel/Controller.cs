using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShipBattlesModel
{
    public class Controller
    {
        public string Username;
        public int level = 0;
        Random rand = new Random();
        public int AIShipSpeed = 1;
        public PlayerShip PlayerShip { get; set; }
        int PlayerSpeed = 3;
        public List<GameObject> hits = new List<GameObject>();

        public void LoadWorld(int lev)
        {
            // Later I will use functions of the 'level' varaible to make the 
            // set up more complicated.

            // Make sure that you set a bullet speed.
            // GameWorld.Instance.Objects.Add(PlayerShip.Intance);
            //GameWorld.Instance.PlayerShip = new PlayerShip();
            //GameWorld.Instance.PlayerShip.ShootDirection = GameWorld.Instance.MakeRandomDirection();
            if(lev == 1)
            {
                GameWorld.Instance.Score = 0;
                GameWorld.Instance.BulletSpeed = 2;
                GameWorld.Instance.Width = 1000;
                GameWorld.Instance.Height = 740;
                PlayerShip = new PlayerShip() { Loc = GetCenterLocation(), Direct = MakeRandDirection() };
                PlayerShip.Speed = PlayerSpeed;
                GameWorld.Instance.PlayerShipLocation = PlayerShip.Loc;
            }

            GameWorld.Instance.Objects.Clear();
            for (int i = 0; i < lev + 4; i++)
            {
                GameWorld.Instance.Objects.Add(new AIShip() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < lev + 3; i++)
            {
                GameWorld.Instance.Objects.Add(new Base() { Loc = MakeRandLocation() });
            }
            for (int i = 0; i < lev + 3; i++)
            {
                GameWorld.Instance.Objects.Add(new Asteroid() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < lev + 3; i++)
            {
                GameWorld.Instance.Objects.Add(new RepairKit() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            GameWorld.Instance.Objects.Add(PlayerShip);


        }

        public List<GameObject> MakePlottibles()
        {
            GameWorld.Instance.Plottibles.Clear();
            foreach (GameObject obj in GameWorld.Instance.Objects)
            {
                if ((Math.Abs(obj.Loc.Y - PlayerShip.Loc.Y) < GameWorld.Instance.Height / 2) && (Math.Abs(obj.Loc.X - PlayerShip.Loc.X) < GameWorld.Instance.Width / 2))
                    GameWorld.Instance.Plottibles.Add(obj);
            }
            return GameWorld.Instance.Plottibles;
        }
        public void IterateGame()
        {
            int i = 0;
            while(i < GameWorld.Instance.Objects.Count)
            {
                GameWorld.Instance.Objects[i].DoNextAction();
                i++;
            }

            hits.Clear();
            foreach(GameObject obj in GameWorld.Instance.Objects)
            {
                if (obj is Bullet)
                {
                    GameObject hitObject = CheckForCollisions(obj);
                    if (hitObject != null)
                    {
                        hits.Add(hitObject);
                        hits.Add(obj); // remove the bullet too.
                    }
                } else if (obj is PlayerBullet)
                {
                    GameObject hitObject = CheckForCollisions(obj);
                    if(hitObject != null)
                    {
                        hits.Add(hitObject);
                        hits.Add(obj); // remove the bullet too.
                    }
                }
            }
            foreach (GameObject obj in hits)
                if (obj != null)
                    obj.GetHit();

            GameWorld.Instance.Plottibles = MakePlottibles();
        }

        private GameObject CheckForCollisions(GameObject obj)
        {
            foreach(GameObject hitObject in GameWorld.Instance.Objects)
            {
                if (obj.Loc.Y < hitObject.Loc.Y + hitObject.CollideBoxSize && obj.Loc.Y > hitObject.Loc.Y - hitObject.CollideBoxSize)
                    if (obj.Loc.X < hitObject.Loc.X + hitObject.CollideBoxSize && obj.Loc.X > hitObject.Loc.X - hitObject.CollideBoxSize)
                        if(hitObject != obj)
                            return hitObject;
            }
            return null;
        }

        public bool IsGameOver()
        {
            if (GameWorld.Instance.Objects.Contains(PlayerShip))
                return false;
            return true;
        }

        public bool IsLevelOver()
        {
            foreach (GameObject obj in GameWorld.Instance.Objects)
                if (obj is Base)
                    return false;
            return true;
        }

        public void Save()
        {
            // https://www.dotnetperls.com/serialize-list
            try
            {
                using (Stream stream = File.Open("SaveFile.txt", FileMode.Create))
                {
                    List<object> saveList = new List<object> { };
                    saveList.Add(Username);
                    saveList.Add(level);
                    saveList.Add(rand);
                    saveList.Add(AIShipSpeed);
                    saveList.Add(PlayerShip);
                    saveList.Add(PlayerSpeed);
                    saveList.Add(hits);
                    saveList.Add(GameWorld.Instance.PlayerShipHitBoxSize);
                    saveList.Add(GameWorld.Instance.PlayerShipLocation);
                    saveList.Add(GameWorld.Instance.Rand);
                    saveList.Add(GameWorld.Instance.Objects);
                    saveList.Add(GameWorld.Instance.Width);
                    saveList.Add(GameWorld.Instance.Height);
                    saveList.Add(GameWorld.Instance.Plottibles);
                    saveList.Add(GameWorld.Instance.BulletSpeed);
                    saveList.Add(GameWorld.Instance.Score);

                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, saveList);
                }
            }
            catch (IOException)
            {
            }
            //Console.WriteLine("Level:");
            //Console.WriteLine(level);
            //Console.WriteLine("Username:");
            //Console.WriteLine(Username);
            //Console.WriteLine("AIShipspeed:");
            //Console.WriteLine(AIShipSpeed);
            //Console.WriteLine("PlayerSpeed");
            //Console.WriteLine(PlayerSpeed);
            // etc
        }

        public void Load()
        {
            // https://www.dotnetperls.com/serialize-list
            try
            {
                using (Stream stream = File.Open("SaveFile.txt", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    List<object> loadList = (List<object>)bin.Deserialize(stream);
                    Username = (string)loadList[0];
                    level = (int)loadList[1];
                    rand = (Random)loadList[2];
                    AIShipSpeed = (int)loadList[3];
                    PlayerShip = (PlayerShip)loadList[4];
                    PlayerSpeed = (int)loadList[5];
                    hits = (List<GameObject>)loadList[6];
                    GameWorld.Instance.PlayerShipHitBoxSize = (int)loadList[7];
                    GameWorld.Instance.PlayerShipLocation = (Location)loadList[8];
                    GameWorld.Instance.Rand = (Random)loadList[9];
                    GameWorld.Instance.Objects = (List<GameObject>)loadList[10];
                    GameWorld.Instance.Width = (int)loadList[11];
                    GameWorld.Instance.Height = (int)loadList[12];
                    GameWorld.Instance.Plottibles = (List<GameObject>)loadList[13];
                    GameWorld.Instance.BulletSpeed = (int)loadList[14];
                    GameWorld.Instance.Score = (int)loadList[15];
                    
                }
            }
            catch (IOException)
            {
            }
        }

        public Direction MakeRandDirection()
        {
            return new Direction() { Up = (rand.Next(3) - 1), Right = (rand.Next(3) - 1) };
        }

        public Location MakeRandLocation()
        {
            return new Location() { X = rand.Next(GameWorld.Instance.Width), Y = rand.Next(GameWorld.Instance.Height) };
        }
        
        public Location GetCenterLocation()
        {
            Location loc = new Location();
            loc.Y = (GameWorld.Instance.Height + GameWorld.Instance.Height % 2) / 2;
            loc.X = (GameWorld.Instance.Width + GameWorld.Instance.Width % 2) / 2;
            return loc;
        }

    }

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
