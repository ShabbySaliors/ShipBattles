using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public abstract class GameObject : ISerializible
    {
        abstract public Location Loc { get; set; }
        abstract public Direction Direct { get; set; }
        abstract public double Speed { get; set; }
        abstract public void UpdatePosition();
        abstract public string Serialize();
        abstract public void Deserialize(string serial);
        
    }

    public class AIShip: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }

        public override void UpdatePosition()
        {

        }
    }

    public class PlayerShip: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }
        public override void UpdatePosition()
        {

        }
    }

    public class Base: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }

        public override void UpdatePosition()
        {

        }
    }

    public class RepairKit: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }

        public override void UpdatePosition()
        {

        }
    }

    public class Asteroid: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }

        public override void UpdatePosition()
        {

        }
    }

    public class PlayerBullet: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }

        public override void UpdatePosition()
        {

        }
    }

    public class Bullet: GameObject, ISerializible
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
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
            Direct.Up = Convert.ToDouble(serialArray[3]);
            Direct.Right = Convert.ToDouble(serialArray[4]);
        }

        public override void UpdatePosition()
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
        public double Up { get; set; } // -1 to 1
        public double Right { get; set; } // -1 to 1
    }
    
}
