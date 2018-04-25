using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class Controller
    {
        private SoundPlayer explosionPlayer = new SoundPlayer("../../Audio/explosion.wav");
        public string Username;
        public int level = 1;
        private bool hasExplosionPlayed = false;
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
            Task.Run(() =>
            {
                if (!PlayerShip.IsInCheatMode)
                {
                    PlayerShip.IsInCheatMode = true;
                    Thread.Sleep(3000);
                    PlayerShip.IsInCheatMode = false;
                }
            });
        }

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
                int dy = GameWorld.Instance.ModY(hitObject.Loc.Y + hitObject.CollideBoxSize) - GameWorld.Instance.ModY(obj.Loc.Y);
                int dx = GameWorld.Instance.ModX(hitObject.Loc.X + hitObject.CollideBoxSize) - GameWorld.Instance.ModX(obj.Loc.X);
                if ((dy < 2 * hitObject.CollideBoxSize && dy > 0) || dy < 2 * hitObject.CollideBoxSize - GameWorld.Instance.Height)
                    if ((dx < 2 * hitObject.CollideBoxSize && dx > 0) || dx < 2 * hitObject.CollideBoxSize - GameWorld.Instance.Width)
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
            if (GameWorld.Instance.Objects.Contains(PlayerShip)) return false;
            if (!hasExplosionPlayed)
            {
                explosionPlayer.Play();
                hasExplosionPlayed = true;
            } 
            return true;
        }

        public bool IsLevelOver()
        {
            foreach (GameObject obj in GameWorld.Instance.Objects)
                if (obj is Base) return false;
            level += 1;
            GameWorld.Instance.Score += (int)(1000 / LevelTimer.Seconds() + PlayerShip.Lives * 3);
            PlayerShip.Lives = 1;
            LoadWorld(level);
            return true;
        }

        /// <summary>
        /// Saves all current game state data into a text file `SaveFile.txt`
        /// </summary>
        public void Save()
        {
            string saveFile = "SaveFile.txt";
            if (File.Exists(saveFile)) File.Delete(saveFile);
            FileStream stream = File.Open(saveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(Username);
            writer.WriteLine(Convert.ToString(level));
            writer.WriteLine(Convert.ToString(GameWorld.Instance.Score));
            writer.WriteLine(Convert.ToString(LevelTimer.currentTime.Subtract(LevelTimer.startingTime)));
            foreach (GameObject gameObject in GameWorld.Instance.Objects)
            {
                string stringObject = gameObject.Serialize();
                writer.WriteLine(stringObject);
            }
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// Loads all saved game state data from `SaveFile.txt` into the 
        /// instance of a new game to give the appearance of seamless 
        /// gameplay from the last saved state
        /// </summary>
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
                    // https://msdn.microsoft.com/en-us/library/se73z7b9(v=vs.110).aspx
                    LevelTimer.startingTime = DateTime.Now.Subtract(TimeSpan.Parse(reader.ReadLine()));
                    LevelTimer.Update();
                    LevelTimer.Seconds();
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
        public DateTime startingTime;
        public DateTime currentTime;
        public int seconds;
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
            seconds = currentTime.Subtract(startingTime).Seconds;
            return seconds;
        }
        public string Write()
        {
            return currentTime.Subtract(startingTime).ToString();
        }
    }
}
