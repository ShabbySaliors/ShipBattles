using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public abstract class GameObject
    {
        abstract public Location Loc { get; set; }
        abstract public Direction Direct { get; set; }
        abstract public double Speed { get; set; }
        abstract public void UpdatePosition();
        abstract public void Serialize();
        abstract public void Deserialize();
        
    }

    public class AIShip: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {
            
        }

        public override void Deserialize()
        {

        }

        public override void UpdatePosition()
        {
            Speed = 9;
        }
    }

    public class PlayerShip: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
        public override void UpdatePosition()
        {

        }
    }

    public class Base: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }

        public override void UpdatePosition()
        {

        }
    }

    public class RepairKit: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }

        public override void UpdatePosition()
        {

        }
    }

    public class Asteroid: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }

        public override void UpdatePosition()
        {

        }
    }

    public class PlayerBullet: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }

        public override void UpdatePosition()
        {

        }
    }

    public class Bullet: GameObject
    {
        public override Location Loc { get; set; }
        public override double Speed { get; set; }
        public override Direction Direct { get; set; }
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

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
