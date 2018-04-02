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

    }

    public class AIShip: GameObject
    {
        
    }

    public class PlayerShip: GameObject
    {

    }

    public class Base: GameObject
    {

    }

    public class RepairKit: GameObject
    {

    }

    public class Asteroid: GameObject
    {

    }

    public class PlayerBullet: GameObject
    {

    }

    public class Bullet: GameObject
    {

    }

    public class Location
    {

    }
    
}
