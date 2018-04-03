using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class GameWorld 
    {
        public List<GameObject> Objects { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PlayerShip PlayerShip { get; set; }

    }
}
