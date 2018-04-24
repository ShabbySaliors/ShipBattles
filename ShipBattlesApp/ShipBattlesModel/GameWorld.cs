using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class GameWorld
    {
        public int Level { get; set; }
        public bool LoadedGame { get; set; }
        public int Score { get; set; }
        public int PlayerShipHitBoxSize { get; set; }
        public Location PlayerShipLocation { get; set; }
        public Random Rand { get; set; }
        public List<GameObject> Objects { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //public PlayerShip PlayerShip;
        public List<GameObject> Plottibles { get; set; }
        public int BulletSpeed { get; set; }

        private GameWorld()
        {
            Rand = new Random();
            Plottibles = new List<GameObject>();
            BulletSpeed = 5;
            Objects = new List<GameObject>();
            Score = 0;
            // Most of the logic for setting up the proper world will be in the controller. 
            // Why? Because we need to incorporate the levels and I'm not sure how do do this
            // in the World constructor. Everytime you make an new level, you will have to effectively
            // make a new world. But be cannot reconstruct the world since it is a singleton. Should it 
            // not be a singleton then? Yes, it must be because we need the objects of the game to check
            // if they are plottable. Of course we could have the controller be a singleton. But we must
            // at least one because the object classes need to reference stuff in the game to make their 
            // disicions. Whatever. I'll just put the World as the singleton and see what happens.
        }

        //public List<GameObject> MakePlottibles()
        //{
        //    foreach(GameObject obj in Objects)
        //    {
        //        if((Math.Abs(obj.Loc.Y - PlayerShip.Loc.Y) < Height / 2) && (Math.Abs(obj.Loc.X - PlayerShip.Loc.X) < Width / 2))
        //            Plottibles.Add(obj);
        //    }
        //    return Plottibles;
        //}

        public Direction MakeRandomDirection ()
        {
            Direction d = new Direction();
            do
            {
                d.Right = Rand.Next(2) - 1;
                d.Up = Rand.Next(2) - 1;
            } while (d.Right == 0 && d.Up == 0);
            return d;
        } 

        public Location MakeRandomLocation()
        {
            Location loc = new Location() { X = Rand.Next(Width), Y = Rand.Next(Height) };
            return loc;
        }

        private static GameWorld instance = new GameWorld();
        public static GameWorld Instance
        {
            get { return instance; }
        }
    }
}
