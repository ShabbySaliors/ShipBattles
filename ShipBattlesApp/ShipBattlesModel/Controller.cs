using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class Controller
    {
        public string Username;
        public GameWorld World { get; set; }
        public int level = 0;
        Random rand = new Random();
        public double AIShipSpeed = 1;
        double PlayerSpeed = 2;

        public void LoadWorld()
        {
            // Later I will use functions of the 'level' varaible to make the 
            // set up more complicated.
            World.Width = 300;
            World.Height = 300;
            for( int i = 0; i < 5; i++)
            {
                World.Objects.Add(new AIShip() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed});
            }
            for (int i = 0; i < 4; i++)
            {
                World.Objects.Add(new Base() { Loc = MakeRandLocation() });
            }
            for (int i = 0; i < 4; i++)
            {
                World.Objects.Add(new Asteroid() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }
            for (int i = 0; i < 3; i++)
            {
                World.Objects.Add(new RepairKit() { Direct = MakeRandDirection(), Loc = MakeRandLocation(), Speed = AIShipSpeed });
            }

            World.PlayerShip = new PlayerShip() { Loc = MakeRandLocation(), Speed = PlayerSpeed };
            World.Objects.Add(World.PlayerShip);
        }

        public void Serialize()
        {
            Console.WriteLine("Level:");
            Console.WriteLine(level);
            Console.WriteLine("Username:");
            Console.WriteLine(Username);
            Console.WriteLine("AIShipspeed:");
            Console.WriteLine(AIShipSpeed);
            Console.WriteLine("PlayerSpeed");
            Console.WriteLine(PlayerSpeed);
            // etc
        }

        public void Deserialize()
        {
            // Some code.
        }

        public Direction MakeRandDirection()
        {
            return new Direction() { Up = (rand.NextDouble() - 0.5) * 2, Right = (rand.NextDouble() - 0.5) };
        }

        public Location MakeRandLocation()
        {
            return new Location() { X = rand.Next(World.Width), Y = rand.Next(World.Height) };
        }
    }
}
