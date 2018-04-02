using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public abstract class GameObject
    {
        Location Loc { get; set; }
        Direction Direct { get; set; }

        abstract public void Serialize();
        abstract public void Deserialize();
        
    }

    public class AIShip: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class PlayerShip: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class Base: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class RepairKit: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class Asteroid: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class PlayerBullet: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class Bullet: GameObject
    {
        public override void Serialize()
        {

        }

        public override void Deserialize()
        {

        }
    }

    public class Location
    {

    }

    public class Direction
    {

    }
    
}
