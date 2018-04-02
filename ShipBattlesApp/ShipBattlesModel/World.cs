using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class GameWorld // Singleton?
    {
        public List<GameObject> Objects { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public GameObject GetPlayersShip()
        {
            foreach (GameObject obj in Objects)
            {
                if obj == 
            }
        }
    }
}
