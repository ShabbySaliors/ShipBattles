using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipBattlesModel
{
    interface ISerializible
    {
        string Serialize();
        void Deserialize(string serial);
    }
}
