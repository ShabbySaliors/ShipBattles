using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
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
                    GameWorld.Instance.Width = 1000; // 1000
                    GameWorld.Instance.Height = 740; // 740
                    PlayerShip = new PlayerShip() { Loc = GetCenterLocation(), Direct = MakeRandDirection() };
                    PlayerShip.Speed = PlayerSpeed;
                    GameWorld.Instance.PlayerShipLocation = PlayerShip.Loc;
                }

            LevelTimer = new LevelTimer();
            GameWorld.Instance.Objects.Clear();
            for (int i = 0; i < lev + 4; i++) // lev + 4
            {
                GameWorld.Instance.Objects.Add(new AIShip() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < lev + 2; i++)
            {
                GameWorld.Instance.Objects.Add(new Base() { Loc = MakeRandLocation() });
            }
            for (int i = 0; i < lev + 3; i++)
            {
                GameWorld.Instance.Objects.Add(new Asteroid() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < lev + 1; i++)
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
                //if ((Math.Abs(obj.Loc.Y % GameWorld.Instance.Height - PlayerShip.Loc.Y % GameWorld.Instance.Height) < GameWorld.Instance.Height / 2)
                //        && (Math.Abs(obj.Loc.X % GameWorld.Instance.Width - PlayerShip.Loc.X % GameWorld.Instance.Width) < GameWorld.Instance.Width / 2))
                //    GameWorld.Instance.Plottibles.Add(obj);
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

            DoPlayerCollisions();

            LevelTimer.Update();
            GameWorld.Instance.Plottibles = MakePlottibles();
        }

        private GameObject CheckForCollisions(GameObject obj)
        {
            foreach(GameObject hitObject in GameWorld.Instance.Objects)
            {
                //if ((obj.Loc.Y % GameWorld.Instance.Height < hitObject.Loc.Y % GameWorld.Instance.Height + hitObject.CollideBoxSize
                //        || obj.Loc.Y % GameWorld.Instance.Height < hitObject.Loc.Y % GameWorld.Instance.Height + hitObject.CollideBoxSize) // if lower than top
                //        && obj.Loc.Y % GameWorld.Instance.Height > hitObject.Loc.Y % GameWorld.Instance.Height - hitObject.CollideBoxSize) // if higher than bottom
                //    if (obj.Loc.X % GameWorld.Instance.Width < hitObject.Loc.X % GameWorld.Instance.Width + hitObject.CollideBoxSize // If 'lefter' than right
                //            && obj.Loc.X % GameWorld.Instance.Width > hitObject.Loc.X % GameWorld.Instance.Width - hitObject.CollideBoxSize) // if 'righter/ than left
                //        if(hitObject != obj)
                //            return hitObject;
                int dy = ((hitObject.Loc.Y + hitObject.CollideBoxSize) % GameWorld.Instance.Height) - (obj.Loc.Y % GameWorld.Instance.Height);
                int dx = ((hitObject.Loc.X + hitObject.CollideBoxSize) % GameWorld.Instance.Width) - (obj.Loc.X % GameWorld.Instance.Width);
                if ((dy < 2 * hitObject.CollideBoxSize && dy > 0) || dy < 2 * hitObject.CollideBoxSize - GameWorld.Instance.Height)
                    if ((dx < 2 * hitObject.CollideBoxSize && dx > 0) || dx < 2 * hitObject.CollideBoxSize -GameWorld.Instance.Width)
                        if (hitObject != obj)
                            return hitObject;
            }
            return null;
        }

        public void DoPlayerCollisions()
        {
            GameObject hitter = CheckForCollisions(PlayerShip);
            if (hitter is Asteroid)
            {
                PlayerShip.GetHit();
                GameWorld.Instance.Objects.Remove(hitter);
            } else if (hitter is RepairKit)
            {
                PlayerShip.Lives += 1;
                GameWorld.Instance.Objects.Remove(hitter);
            }
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
            GameWorld.Instance.Score += (int)(1000 / LevelTimer.Seconds() + PlayerShip.Lives * 3);
            PlayerShip.Lives = 1;
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
            if (File.Exists(saveFile))
            {
                using (FileStream stream = File.Open(saveFile, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);
                    Username = reader.ReadLine();
                    level = Convert.ToInt32(reader.ReadLine());
                    LoadWorld(level);
                    GameWorld.Instance.Score = Convert.ToInt32(reader.ReadLine());
                    GameWorld.Instance.Objects.Clear();
                    string nextObj = reader.ReadLine();
                    while (nextObj != null)
                    {
                        Location loc = MakeRandLocation();
                        Direction dir = MakeRandDirection();
                        string[] tempArray = nextObj.Split(',');
                        int objectType = Convert.ToInt32(tempArray[5]);
                        switch (objectType)
                        {
                            case 0:
                                PlayerShip p = new PlayerShip() { Loc = loc, Direct = dir, Speed = 1, Lives = 1 };
                                PlayerShip = p;
                                p.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(p);
                                break;
                            case 1:
                                AIShip a = new AIShip() { Loc = loc, Direct = dir, Speed = 1 };
                                a.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(a);
                                break;
                            case 2:
                                Base b = new Base() { Loc = loc, Direct = dir, Speed = 1, CollideBoxSize = 1 };
                                b.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(b);
                                break;
                            case 3:
                                RepairKit r = new RepairKit() { Loc = loc, Direct = dir, Speed = 1 };
                                r.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(r);
                                break;
                            case 4:
                                Asteroid ast = new Asteroid() { Loc = loc, Direct = dir, Speed = 1 };
                                ast.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(ast);
                                break;
                            case 5:
                                PlayerBullet pb = new PlayerBullet() { Loc = loc, Direct = dir, Speed = 1, numberOfMoves = 0 };
                                pb.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(pb);
                                break;
                            case 6:
                                Bullet bult = new Bullet() { Loc = loc, Direct = dir, Speed = 1, numberOfMoves = 0 };
                                bult.Deserialize(nextObj);
                                GameWorld.Instance.Objects.Add(bult);
                                break;
                        }
                        nextObj = reader.ReadLine();
                    }
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
