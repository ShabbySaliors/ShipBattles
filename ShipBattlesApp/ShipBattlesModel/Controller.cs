﻿//-----------------------------------------------------
//File:   Controller.cs
//Desc:   This file contains code that controls
//        all of the game world objects in ShipBattles.
//-----------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    /// <summary>
    /// The controller contains all the main logic for the game including the main game loop, IterateGame().
    /// </summary>
    public class Controller
    {
        private SoundPlayer explosionPlayer = new SoundPlayer("../../Audio/explosion.wav"); // the sound for the exposion.
        public string Username;   // the username for highscores
        public int level = 1;     // the initial level is 1. But this varaible increased later on.
        private bool hasExplosionPlayed = false;  // I don't know.
        public LevelTimer LevelTimer {get; set;}  // keeps track of how long the level has been happening
        Random rand = new Random();               // A random variable for a bunch of random stuff
        public int AIShipSpeed = 1;               // the speed of the AIShips. Placed here because at one time
                                                  // we were concidering making it vary depending of levels. 
        public PlayerShip PlayerShip { get; set; } // the player's ship.
        int PlayerSpeed = 3;                        // speed of the player's ship
        public List<GameObject> hits = new List<GameObject>(); // a list of objects which were calculated to 
                                                               // have been hit during this iteration of the game.
        /// <summary>
        /// This method laods everything necessary into the GameWorld.Instance to make the game go. 
        /// It loads more object for higher levels.
        /// </summary>
        /// <param name="lev"></param>
        public void LoadWorld(int lev)
        {
            // if the level is 1, then the inital conditions are set.
            if(lev == 1)
                {
                    GameWorld.Instance.Score = 0;
                    GameWorld.Instance.BulletSpeed = 2;
                    GameWorld.Instance.Width = 1200; 
                    GameWorld.Instance.Height = 900; 
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
         
        /// <summary>
        /// This method adds all the object which should be added into a list.
        /// It used to distinguish more, but now it just adds them all. HOwever, it would have
        /// borken code to delete the method. 
        /// </summary>
        /// <returns></returns>
        public List<GameObject> MakePlottibles()
        {
            GameWorld.Instance.Plottibles.Clear();
            foreach (GameObject obj in GameWorld.Instance.Objects)
            {
                GameWorld.Instance.Plottibles.Add(obj);
            }
            return GameWorld.Instance.Plottibles;
        }

        /// <summary>
        /// The main game loop
        /// </summary>
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


        /// <summary>
        /// Checks for collisions with an object and returns the other object colliding with it.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This increases the lives if the player collides with a repairkit and decreases the lives if
        /// the player collides with an asteroid
        /// </summary>
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

        /// <summary>
        /// checks to see if the game is over and if it is, plays a sound.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks see if the level is over and if it is, starts the next one.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// returns a random direction
        /// </summary>
        /// <returns></returns>
        public Direction MakeRandDirection()
        {
            return new Direction() { Up = (rand.Next(3) - 1), Right = (rand.Next(3) - 1) };
        }

        /// <summary>
        /// Makes a random location
        /// </summary>
        /// <returns></returns>
        public Location MakeRandLocation()
        {
            return new Location() { X = rand.Next(GameWorld.Instance.Width), Y = rand.Next(GameWorld.Instance.Height) };
        }
        
        /// <summary>
        /// Makes the center location of the board. Was a useful method without the infinite world.
        /// </summary>
        /// <returns></returns>
        public Location GetCenterLocation()
        {
            Location loc = new Location();
            loc.Y = (GameWorld.Instance.Height + GameWorld.Instance.Height % 2) / 2;
            loc.X = (GameWorld.Instance.Width + GameWorld.Instance.Width % 2) / 2;
            return loc;
        }
    }

    /// <summary>
    /// This class contians the data for calculating the time for each level.
    /// </summary>
    public class LevelTimer
    {
        public DateTime startingTime;       // the time that the level started
        public DateTime currentTime;        // the current time
        public int seconds;                 // the number of seconds between them.

        /// <summary>
        /// Sets the starting time to now.
        /// </summary>
        public LevelTimer()
        {
            startingTime = DateTime.Now;
        }

        /// <summary>
        /// Sets the current time to now.
        /// </summary>
        public void Update()
        {
            currentTime = DateTime.Now;
        }
        
        /// <summary>
        /// returns the number of seconds since the level started. 
        /// </summary>
        /// <returns></returns>
        public int Seconds()
        {
            seconds = currentTime.Subtract(startingTime).Seconds;
            return seconds;
        }

        /// <summary>
        /// for serialization.
        /// </summary>
        /// <returns></returns>
        public string Write()
        {
            return currentTime.Subtract(startingTime).ToString();
        }
    }
}
