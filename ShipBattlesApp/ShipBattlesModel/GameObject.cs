using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public abstract class GameObject : ISerializible
    {
        abstract public string ImageFilepath { get; set; }
        abstract public int CollideBoxSize { get; set; }
        abstract public int HitBoxSize { get; set; }
        abstract public Location Loc { get; set; }
        abstract public void DoNextAction();
        abstract public string Serialize();
        abstract public void Deserialize(string serial);
        abstract public GameObject GetHit();
    }

    public class AIShip: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int CollideBoxSize { get; set; }
        public override int HitBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }
        public AIShip()
        {
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/alien.png";
        }
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

        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

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

        public void Turn()
        {
            Location target = GameWorld.Instance.PlayerShipLocation;
            // Here we would like to point the AIShip in the direction of the player's ship. 
            // To do this, we will check each case where the players coordinates are in different 
            // quadrents of the AIships origin. 
            if(target.Y - GameWorld.Instance.PlayerShipHitBoxSize > Loc.Y)
                Direct.Up = 1;
            else if (target.Y + GameWorld.Instance.PlayerShipHitBoxSize < Loc.Y)
                Direct.Up = -1;
            else
                Direct.Up = 0;

            if (target.X - GameWorld.Instance.PlayerShipHitBoxSize > Loc.X)
                Direct.Right = 1;
            else if (target.X + GameWorld.Instance.PlayerShipHitBoxSize < Loc.X)
                Direct.Right = -1;
            else if (Direct.Up != 0)
                Direct.Right = 0;
            else
                Direct.Right = 1; // This is merely to handle that case where the algorithm would 
                                  // place both in the components of the direction to 0. 
        }

        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

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
        public override GameObject GetHit()
        {
            GameWorld.Instance.Score += 1;
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    public class PlayerShip : GameObject, ISerializible
    {
        public int Lives { get; set; }
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public bool ToShoot { get; set; }
        public Direction ShootDirection { get; set; } // here!
        public Direction Direct { get; set; }
        public bool IsInCheatMode = false;
        public PlayerShip()
        {
            Lives = 1;
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/playerShip.png";
            ToShoot = false;
            ShootDirection = new Direction { Up = 1, Right = 0 };
        }

        public PlayerShip Callibrate(int level)
        {
            Loc = GameWorld.Instance.MakeRandomLocation();
            Direct = GameWorld.Instance.MakeRandomDirection();
            Speed = 2;
            return this;
        }
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
                ToShoot = false;
            }
        }
        private void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }

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

        public override string Serialize() // Make is a single String
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

        //private static PlayerShip instance = new PlayerShip();
        //public static PlayerShip Instance
        //{
        //    get { return instance; }
        //}
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

    public class Base: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public double Speed { get; set; }
        public Direction Direct { get; set; }
        public Base()
        {
            CollideBoxSize = 40;
            HitBoxSize = 30;
            ImageFilepath = "Images/SpaceStation.png";
        }
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

        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToDouble(serialArray[2]);
            CollideBoxSize = Convert.ToInt32(serialArray[3]);
        }

        public override void DoNextAction()
        {
            
        }
        public override GameObject GetHit()
         {
            CollideBoxSize -= 4;
            GameWorld.Instance.Score += 1;
            if(CollideBoxSize < 5)
                GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    public class RepairKit: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }
        public RepairKit()
        {
            Speed = 1;
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/life.png";
        }
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

        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

        public override void DoNextAction()
        {
            double r = rand.NextDouble();
            if (r < 0.05)
                Turn();
            else
                Move();
        }

        public void Turn()
        {
            Direct = GameWorld.Instance.MakeRandomDirection();
        }

        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

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

        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToInt32(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

        public override void DoNextAction()
        {
            double r = rand.NextDouble();
            if (r < 0.05)
                Turn();
            else
                Move();
        }

        public void Turn()
        {
            Direct = GameWorld.Instance.MakeRandomDirection();
        }

        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

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
        public void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }

    }

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
        public override string Serialize() // Make is a single String
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

        private void Move()
        {
            Loc.X += Speed * Direct.Right;
            Loc.Y += Speed * Direct.Up;
        }
        public override GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Direction
    {
        public int Up { get; set; } // -1, 0, 1
        public int Right { get; set; } // -1, 0,  1

        // But it cannot be (0, 0).
    }
    
}
