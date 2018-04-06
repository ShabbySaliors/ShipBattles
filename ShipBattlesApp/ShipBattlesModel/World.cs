using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class GameWorld 
    {
        public Random Rand { get; set; }
        public List<GameObject> Objects { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PlayerShip PlayerShip { get; set; }

        private GameWorld()
        {
            Rand = new Random();
            // Most of the logic for setting up the proper world will be in the controller. 
            // Why? Because we need to incorporate the levels and I'm not sure how do do this
            // in the World constructor. Everytime you make an new level, you will have to effectively
            // make a new world. But be cannot reconstruct the world since it is a singleton. Should it 
            // not be a singleton then? Yes, it must be because we need the objects of the game to check
            // if they are plottable. Of course we could have the controller be a singleton. But we must
            // at least one because the object classes need to reference stuff in the game to make their 
            // disicions.
        }
    }
}
