using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShipBattlesModel
{
    interface ISerializible
    {
        string Serialize();
        void Deserialize(string serial);
    }
}
