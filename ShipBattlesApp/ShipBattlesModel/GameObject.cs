using System;

namespace ShipBattlesModel
{
    /// <summary>
    /// Parent class for all in-game objects
    /// </summary>
    public abstract class GameObject : ISerializible
    {
        abstract public string ImageFilepath { get; set; } // a path for image
        abstract public int CollideBoxSize { get; set; } // The size of half an edge of the box 
                                                         // which is used for detecting collisions
        abstract public int HitBoxSize { get; set; }    // size of half an edge of the bos which 
                                                        // is used for detecting hits from a bullet.
        abstract public Location Loc { get; set; }      // The location of the GameObject

        /// <summary>
        /// A method which is called in the main game loop on each WorldObject. The object then 
        /// moves or shoots according to its own logic
        /// </summary>
        abstract public void DoNextAction();

        /// <summary>
        /// Compiles all the necesary information about the object into a string so that it can 
        /// later be accessed by the Deserialize(string serial) method
        /// </summary>
        /// <returns></returns>
        abstract public string Serialize();

        /// <summary>
        /// This method takes a string with information and the object uses that information 
        /// to reset its properties and instance variables.
        /// </summary>
        /// <param name="serial"></param>
        abstract public void Deserialize(string serial);

        /// <summary>
        /// This method is called whenever a bullet hits the object. Usually the object is removed
        /// from the GameWorld.Instance
        /// </summary>
        /// <returns></returns>
        abstract public GameObject GetHit();
    }

    /// <summary>
    /// The class of ailien ships.
    /// </summary>
    public class AIShip: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int CollideBoxSize { get; set; }
        public override int HitBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;  // a random variable for the some AI operations
        public override Location Loc { get; set; } 
        public int Speed { get; set; }                  // An integer assinging the distance the ship moves
                                                        // each time MOve() is called.
        public Direction Direct { get; set; }           // The direction of movement.

        /// <summary>
        /// A constructor which assigns the imagepath, the HitBoxSize, and the CollideBoxSize
        /// </summary>
        public AIShip()
        {
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/alien.png";
        }
        
        /// <summary>
        /// Compiles `AIShip` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize() // Make it a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right) + ",";
            serial += "1";
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `AIShip` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

        /// <summary>
        /// Here is the AI logic of the ship. 
        ///     - 90% of the time, the ship will move
        ///     - 5% of the time, the ship will turn
        ///     - 5% of the time, the ship will shoot
        /// </summary>
        public override void DoNextAction()
        {
            double r = rand.NextDouble();
            if (r < 0.05)
                this.Turn();
            else if (r < 0.95)
                Move();
            else
                Shoot();
        }

        /// <summary>
        /// This logic is rather complicated to explain. The basic idea is to use mod math to 
        /// get the relative placement of the players ship to the AIship.
        /// We want to point the AIShip in the direction of the player's ship. 
        /// To do this, we will check each case where the players coordinates are in different 
        /// quadrents of the AIships origin and asign a direction accrodingly.
        /// </summary>
        public void Turn()
        {
            Location target = GameWorld.Instance.PlayerShipLocation;
            int dy = GameWorld.Instance.ModY(target.Y + GameWorld.Instance.PlayerShipHitBoxSize) - GameWorld.Instance.ModY(Loc.Y);
            int dx = GameWorld.Instance.ModX(target.X + GameWorld.Instance.PlayerShipHitBoxSize) - GameWorld.Instance.ModX(Loc.X);
            if ((dy < 2 * GameWorld.Instance.PlayerShipHitBoxSize && dy >= 0) || dy < 2 * GameWorld.Instance.PlayerShipHitBoxSize - GameWorld.Instance.Height)
                Direct.Up = 0;
            else if (Math.Abs(dy) == 0)
                return;
            else if (Math.Abs(dy) < GameWorld.Instance.Height / 2)
                Direct.Up = dy / Math.Abs(dy); // sign (+-) of dy    { -1, 1 } 
            else
                Direct.Up = -dy / Math.Abs(dy); // opposite sign of dy { 1, -1 } 

            if ((dx < 2 * GameWorld.Instance.PlayerShipHitBoxSize && dx >= 0) || dx < 2 * GameWorld.Instance.PlayerShipHitBoxSize - GameWorld.Instance.Width)
                Direct.Right = 0;
            else if (Math.Abs(dx) == 0)
                return;
            else if (Math.Abs(dx) < GameWorld.Instance.Width / 2)
                Direct.Right = dx / Math.Abs(dx); // sign (+-) of dx    { -1, 1 } 
            else
                Direct.Right = -dx / Math.Abs(dx); // opposite sign of dx { 1, -1 } 
        }

        /// <summary>
        /// This method takes moves the AIship one graph unit in its Direct (direction).
        /// </summary>
        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }
        /// <summary>
        /// Creates a bullet that heads in the direction that the ship is heading. 
        /// </summary>
        /// <returns></returns>
        public Bullet Shoot()
        {
            Bullet b = new Bullet();
            b.Direct = new Direction();
            b.Direct.Up = Direct.Up;
            b.Direct.Right = Direct.Right;
            b.Loc = new Location();
            b.Loc.Y = Loc.Y + Direct.Up * (HitBoxSize + 10);
            b.Loc.X = Loc.X + Direct.Right * (HitBoxSize + 10);
            GameWorld.Instance.Objects.Add(b);
            return b;
        }
        

        /// <summary>
        /// This method is called whenever the ship gets hit. It removes the ship from the game
        /// and increases the score by 1 point.
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
        {
            GameWorld.Instance.Score += 1;
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    /// <summary>
    /// The class of the player's ship which he controls to win the game.
    /// </summary>
    public class PlayerShip : GameObject, ISerializible
    {
        public int Lives { get; set; }                  // The number of lives that the player has left. 
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public bool ToShoot { get; set; }               // In the DoNextAction method, we check if the player is supposed to shoot. 
        public Direction ShootDirection { get; set; }   // This is the direction which the bullets shoot. 
                                                        // It had to be different from the player's moving
                                                        // direction because the ship is not always moving 
                                                        // and therefore does not always have a non-zero direction.
                                                        // This direction is passed onto the bullet. We would 
                                                        // not want the bullet to just sit there.
        public Direction Direct { get; set; }
        public bool IsInCheatMode = false;              // This bool lets the controler know whether the ship is destructible.
        public PlayerShip()
        {
            Lives = 1;
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/playerShip.png";
            ToShoot = false;                            // We set ToShoot to false initially so that the ship 
                                                        // does not shoot whenever the game starts up.
            ShootDirection = new Direction { Up = 1, Right = 0 }; // the initial direction is up for the bullets
        }
        /// <summary>
        /// This method is called by the contoller at the begining of each level. The ship is randomly placed.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public PlayerShip Callibrate(int level)
        {
            Loc = GameWorld.Instance.MakeRandomLocation();
            Direct = GameWorld.Instance.MakeRandomDirection();
            Speed = 2;
            return this;
        }

        /// <summary>
        /// Called by the main game loop upon each iteration, this method performs what action 
        /// the user has selected to be next. 
        /// </summary>
        public override void DoNextAction()
        {
            Move();
            if (!(Direct.Up == 0 && Direct.Right == 0))
            {
                ShootDirection.Up = Direct.Up;
                ShootDirection.Right = Direct.Right;
            }
            if(ToShoot)
            {
                 Shoot();
                ToShoot = false; // so that we aren't shooting continually.
            }
        }
        
        /// <summary>
        /// This method moves the object one graph unit in the direction of Direct.
        /// </summary>
        private void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

        /// <summary>
        /// Creates a PlayerBullet (which moves faster than other bullets) and sets it in the direction
        /// which the Playership is pointing.
        /// </summary>
        /// <returns></returns>
        public PlayerBullet Shoot()
        {
            PlayerBullet b = new PlayerBullet();
            b.Direct = new Direction();
            b.Direct.Up = ShootDirection.Up;
            b.Direct.Right = ShootDirection.Right;
            b.Loc = new Location();
            b.Loc.Y = Loc.Y + ShootDirection.Up * (HitBoxSize + 5);
            b.Loc.X = Loc.X + ShootDirection.Right * (HitBoxSize + 5);
            GameWorld.Instance.Objects.Add(b);
            return b;
        }

        /// <summary>
        /// This method gets some object relative position to the playership in the y direction. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int FindY_Dist(GameObject obj)
        {
            int d = obj.Loc.Y % GameWorld.Instance.Height - Loc.Y % GameWorld.Instance.Height;
            if (d < 0 - GameWorld.Instance.Height / 2)
                return d + GameWorld.Instance.Height;
            else if (d > GameWorld.Instance.Height / 2)
                return d - GameWorld.Instance.Height;
            else
                return d;
        }

        /// <summary>
        /// This method gets some object relative position to the playership in the x direction. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int FindX_Dist(GameObject obj)
        {
            int d = obj.Loc.X % GameWorld.Instance.Width - Loc.X % GameWorld.Instance.Width;
            if (d < 0 - GameWorld.Instance.Width / 2)
                return d + GameWorld.Instance.Width;
            else if (d > GameWorld.Instance.Width / 2)
                return d - GameWorld.Instance.Width;
            else
                return d;
        }

        /// <summary>
        /// Compiles `PlayerShip` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize()
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right) + ",";
            serial += "0,";
            serial += Convert.ToString(Lives);
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `PlayerShip` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
            Lives = Convert.ToInt32(serialArray[6]);
        }

        /// <summary>
        /// Called whenever a bullet or an asteroid collides with the playership, this method
        /// decreases the number of lives. If the player has only one life left, then it is 
        /// removed from the game and the game is over.
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
        {
            if (!IsInCheatMode)
            {
                if (Lives == 1)
                {
                    GameWorld.Instance.Objects.Remove(this);
                    return this;
                } else
                {
                    Lives -= 1;
                }

            }
            return null;
        }
    }

    /// <summary>
    /// Class for the Bases, which are the object of the game.
    /// </summary>
    public class Base: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        public override Location Loc { get; set; }
        public double Speed { get; set; }
        public Direction Direct { get; set; }

        /// <summary>
        /// Sets the image, the collidebox size and the hitbox size for the Base()
        /// </summary>
        public Base()
        {
            CollideBoxSize = 40;
            HitBoxSize = 30;
            ImageFilepath = "Images/SpaceStation.png";
        }

        /// <summary>
        /// Compiles `Base` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(CollideBoxSize) + ",";
            serial += "0,";
            serial += "2";
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `Base` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToDouble(serialArray[2]);
            CollideBoxSize = Convert.ToInt32(serialArray[3]);
        }

        /// <summary>
        /// Bases are passive. But I still need the method for how the main game loop works
        /// </summary>
        public override void DoNextAction()
        {
            
        }

        /// <summary>
        /// If the base is hit by a bullet, then the CollideBoxSize shrinks. When the size 
        /// reaches a certain minimum, the based is destroyed and removed from the game.
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
         {
            CollideBoxSize -= 4;
            GameWorld.Instance.Score += 1;
            if(CollideBoxSize < 5)
                GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    /// <summary>
    /// The purpose of the repairkit is to give the player some extra lives. When the player collides with 
    /// one of these, its lives increase by one and the repairKit disapears.
    /// </summary>
    public class RepairKit: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;      // To make it wander.
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }

        /// <summary>
        /// Sets the speed, collideBoxSize, HitBoxSize and the Image.
        /// </summary>
        public RepairKit()
        {
            Speed = 1;
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/life.png";
        }

        /// <summary>
        /// Compiles `RepairKit` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right) + ",";
            serial += "3";
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `RepairKit` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

        /// <summary>
        /// Causes the repairkit to walk randomly on the GameWorld plain. 
        /// 5% of the time, the repairkit changes its direction of movement. 
        /// </summary>
        public override void DoNextAction()
        {
            double r = rand.NextDouble();
            if (r < 0.05)
                Turn();
            else
                Move();
        }

        /// <summary>
        /// Selects a random direction.
        /// </summary>
        public void Turn()
        {
            Direct = GameWorld.Instance.MakeRandomDirection();
        }

        /// <summary>
        /// Moves the object one graph unit in its direction of travel.
        /// </summary>
        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

        /// <summary>
        /// if a repairkit is shot, it will be removed from the game.
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    /// <summary>
    /// If the player collides with an asteroid, he is destroyed and the game is over.
    /// Asteroids are indestrucitble (unless hit by the player).
    /// </summary>
    public class Asteroid: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }
        public Asteroid()
        {
            Speed = 1;
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/asteroid.png";
        }

        /// <summary>
        /// Compiles `Asteroid` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right) + ",";
            serial += "4";
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `Asteroid` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

        /// <summary>
        /// selects a random direction
        /// </summary>
        public override void DoNextAction()
        {
            double r = rand.NextDouble();
            if (r < 0.05)
                Turn();
            else
                Move();
        }

        /// <summary>
        /// Turns a random direction
        /// </summary>
        public void Turn()
        {
            Direct = GameWorld.Instance.MakeRandomDirection();
        }

        /// <summary>
        /// Moves the object one graph unit in the direction of its motion.
        /// </summary>
        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

        /// <summary>
        /// If an asteroid is hit, only the bullet is destroyed.
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
        {
            return this;
        }
    }

    /// <summary>
    /// The player bullet has its own class becuase it has a different speed than the other bullets.
    /// </summary>
    public class PlayerBullet: GameObject, ISerializible
    {
        public int numberOfMoves = 0;
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }
        public PlayerBullet()
        {
            CollideBoxSize = 3;
            HitBoxSize = 1;
            ImageFilepath = "Images/laser.png";
            Speed = 5;
        }

        /// <summary>
        /// Compiles `PlayerBullet` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right) + ",";
            serial += "5,";
            serial += Convert.ToString(numberOfMoves);
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `PlayerBullet` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
            numberOfMoves = Convert.ToInt32(serialArray[6]);
        }

        /// <summary>
        /// Moves the bullet if it has not yet exhausted its moves. If it has, the method destroyes the bullet.
        /// </summary>
        public override void DoNextAction()
        {
            if (numberOfMoves < GameWorld.Instance.Height / Speed / 2)
            {
                Move();
                numberOfMoves += 1;
            }
            else
            {
                GameWorld.Instance.Objects.Remove(this);
            }
        }

        /// <summary>
        /// Moves the object one graph unit in the direction of its travel.
        /// </summary>
        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

        /// <summary>
        /// If hit, it is removed
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }

    }

    /// <summary>
    /// The class of bullets produced by AIships.
    /// </summary>
    public class Bullet: GameObject, ISerializible
    {
        public int numberOfMoves = 0;
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }

        public Bullet()
        {
            Speed = GameWorld.Instance.BulletSpeed; // Assumes that that bullet speed has been set by the contoller
            HitBoxSize = 1;
            CollideBoxSize = 3;
            ImageFilepath = "Images/laser.png";
        }

        /// <summary>
        /// Compiles `Bullet` components into a single string
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        public override string Serialize()
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right) + ",";
            serial += "6,";
            serial += Convert.ToString(numberOfMoves);
            return serial;
        }
        /// <summary>
        /// Breaks apart a comma-delineated string and
        /// sets the appropriate `Bullet` properties
        /// </summary>
        /// <param name="serial">Comma-delineated</param>
        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
            numberOfMoves = Convert.ToInt32(serialArray[6]);
        }

        /// <summary>
        /// If the bullet has not exhausted its moves, it moves.
        /// Else, the bullet is destroyed
        /// </summary>
        public override void DoNextAction()
        {
            if (numberOfMoves < GameWorld.Instance.Height / Speed / 2)
            {
                Move();
                numberOfMoves += 1;
            }
            else
                GameWorld.Instance.Objects.Remove(this);
        }

        /// <summary>
        /// Moves the bullet 2 graph units
        /// </summary>
        private void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

        /// <summary>
        /// If hit, the bullet is destroyed
        /// </summary>
        /// <returns></returns>
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    /// <summary>
    /// This is the class which can contain a coordinate for various purposes. 
    /// Used primarily as Properties of GameObjects to place the data of where they are at.
    /// </summary>
    public class Location
    {
        public int X { get; set; } // the positon on the x-axis
        public int Y { get; set; } // the position on the y-axis
    }

    /// <summary>
    /// Contains the data for direcion of movement. 
    /// </summary>
    public class Direction
    {
        public int Up { get; set; } // -1, 0, 1
        public int Right { get; set; } // -1, 0,  1

        // But it cannot be (0, 0).
    }
    
}
