using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;

namespace ShipBattlesModel
{
    public class Controller
    {
        private SoundPlayer explosionPlayer = new SoundPlayer("Audio/explosion.wav");
        public string Username;
        public int level = 1;
        public LevelTimer LevelTimer {get; set;}
        Random rand = new Random();
        public int AIShipSpeed = 1;
        public PlayerShip PlayerShip { get; set; }
        int PlayerSpeed = 3;
        public List<GameObject> hits = new List<GameObject>();
        public void LoadWorld(int lev)
        {

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

            LevelTimer = new LevelTimer();
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
        
        // Load Logic:
        // Call the LoadWorld method once with lev = 1;
        // Call it again with current level;
        // Clear Gameworld.Object;
        // Populate Gameworld.Objects with your info from the file

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
                        explosionPlayer.Play();
                        hits.Add(hitObject);
                        hits.Add(obj); // remove the bullet too.
                    }
                } else if (obj is PlayerBullet)
                {
                    GameObject hitObject = CheckForCollisions(obj);
                    if(hitObject != null)
                    {
                        explosionPlayer.Play();
                        hits.Add(hitObject);
                        hits.Add(obj); // remove the bullet too.
                    }
                }
            }
            foreach (GameObject obj in hits)
                if (obj != null)
                    obj.GetHit();
            LevelTimer.Update();
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
            level += 1;
            GameWorld.Instance.Score += (int)(1000 / LevelTimer.Seconds());
            LoadWorld(level);
            return true;
        }

        public void Save()
        {
            string saveFile = "SaveFile.txt";
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }
            FileStream stream = File.Open(saveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(Username);
            writer.WriteLine(Convert.ToString(level));
            writer.WriteLine(Convert.ToString(PlayerShip.Loc.X));
            writer.WriteLine(Convert.ToString(PlayerShip.Loc.Y));
            //writer.WriteLine(GameWorld.Instance.PlayerShipLocation);
            writer.WriteLine(Convert.ToString(GameWorld.Instance.Score));
            foreach (GameObject gameObject in GameWorld.Instance.Objects)
            {
                string stringObject = gameObject.Serialize();
                writer.WriteLine(stringObject);
            }
            writer.Close();
            stream.Close();
        }

        public void Load()
        {
            string saveFile = "SaveFile.txt";
            if (File.Exists(saveFile) == true)
            {
                using (FileStream stream = File.Open(saveFile, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);
                    Username = reader.ReadLine();
                    level = Convert.ToInt32(reader.ReadLine());
                    PlayerShip.Loc.X = Convert.ToInt32(reader.ReadLine());
                    PlayerShip.Loc.Y = Convert.ToInt32(reader.ReadLine());
                    //GameWorld.Instance.PlayerShipLocation = (Location)reader.ReadLine();
                    GameWorld.Instance.Score = Convert.ToInt32(reader.ReadLine());
                    //while (reader.ReadLine() != null)
                    //{
                        //gameObject.Deserialize(reader.ReadLine());
                        //GameWorld.Instance.Objects.Add(gameObject);
                    //}
                    reader.Close();
                    stream.Close();
                }
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

    public class LevelTimer
    {
        DateTime startingTime;
        DateTime currentTime;
        public LevelTimer()
        {
            startingTime = DateTime.Now;
        }
        public void Update()
        {
            currentTime = DateTime.Now;
        }
        public int Seconds()
        {
            return currentTime.Subtract(startingTime).Seconds;
        }
        public string Write()
        {
            return currentTime.Subtract(startingTime).ToString();
        }
    }
}
