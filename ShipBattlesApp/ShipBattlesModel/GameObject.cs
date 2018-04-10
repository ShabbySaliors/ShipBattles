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
        public GameObject GetHit()
        {
            GameWorld.Instance.Objects.Remove(this);
            return this;
        }
    }

    public class AIShip: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int CollideBoxSize { get; set; }
        public override int HitBoxSize { get; set; }
        public int hitBoxSize = 10;
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }
        public AIShip()
        {
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/alien.jpg";
        }
        public override string Serialize() // Make it a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
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
            Location target = GameWorld.Instance.PlayerShip.Loc;
            // Here we would like to point the AIShip in the direction of the player's ship. 
            // To do this, we will check each case where the players coordinates are in different 
            // quadrents of the AIships origin. 
            if(target.Y - GameWorld.Instance.PlayerShip.HitBoxSize > Loc.Y)
                Direct.Up = 1;
            else if (target.Y + GameWorld.Instance.PlayerShip.HitBoxSize < Loc.Y)
                Direct.Up = -1;
            else
                Direct.Up = 0;

            if (target.X - GameWorld.Instance.PlayerShip.HitBoxSize > Loc.X)
                Direct.Right = 1;
            else if (target.X + GameWorld.Instance.PlayerShip.HitBoxSize < Loc.X)
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
            b.Direct = Direct;
            b.Loc = Loc;
            b.Loc.Y += Direct.Up * (hitBoxSize + 1);
            b.Loc.X += Direct.Right * (hitBoxSize + 1);
            GameWorld.Instance.Objects.Add(b);
            return b;
        }
    }

    public class PlayerShip: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public int Speed { get; set; }
        public Direction Direct { get; set; }
        private PlayerShip()
        {
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/playerShip.png";
        }

        public PlayerShip Callibrate(int level)
        {
            Loc = GameWorld.Instance.MakeRandomLocation();
            Direct = GameWorld.Instance.MakeRandomDirection();
            Speed = 2;
            return this;
        }
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
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
            
        }
        private static PlayerShip instance = new PlayerShip();
        public static PlayerShip Instance
        {
            get { return instance; }
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
            ImageFilepath = "Images/SpaceStation.jpg";
        }
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
            return serial;
        }

        public override void Deserialize(string serial)
        {
            string[] serialArray = serial.Split(',');
            Loc.Y = Convert.ToInt32(serialArray[0]);
            Loc.X = Convert.ToInt32(serialArray[1]);
            Speed = Convert.ToDouble(serialArray[2]);
            Direct.Up = Convert.ToInt32(serialArray[3]);
            Direct.Right = Convert.ToInt32(serialArray[4]);
        }

        public override void DoNextAction()
        {

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
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "logo.png";
        }
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
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
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/asteroid.jpg";
        }
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
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
    }

    public class PlayerBullet: GameObject, ISerializible
    {
        public override string ImageFilepath { get; set; }
        public override int HitBoxSize { get; set; }
        public override int CollideBoxSize { get; set; }
        private Random rand = GameWorld.Instance.Rand;
        public override Location Loc { get; set; }
        public double Speed { get; set; }
        public Direction Direct { get; set; }
        public PlayerBullet()
        {
            CollideBoxSize = 20;
            HitBoxSize = 10;
            ImageFilepath = "Images/laser.jpg";
        }
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
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

        }
    }

    public class Bullet: GameObject, ISerializible
    {
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
            CollideBoxSize = 2;
            ImageFilepath = "laser.jpg";
        }
        public override string Serialize() // Make is a single String
        {
            string serial = "";
            serial += Convert.ToString(Loc.Y) + ",";
            serial += Convert.ToString(Loc.X) + ",";
            serial += Convert.ToString(Speed) + ",";
            serial += Convert.ToString(Direct.Up) + ",";
            serial += Convert.ToString(Direct.Right);
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
