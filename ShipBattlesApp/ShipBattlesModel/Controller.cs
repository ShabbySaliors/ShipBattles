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
            LoadWorld(level);
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
        public string Write()
        {
            return currentTime.Subtract(startingTime).ToString();
        }
    }
}
