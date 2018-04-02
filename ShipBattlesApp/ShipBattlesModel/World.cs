using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    public class GameWorld // Singleton?
    {
        List<GameObject> Objects { get; set; }
        int Width { get; set; }
        int Height { get; set; }

    }
}
