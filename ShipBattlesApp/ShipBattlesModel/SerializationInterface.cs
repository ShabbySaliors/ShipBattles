
namespace ShipBattlesModel
{
    interface ISerializible
    {
        string Serialize();
        void Deserialize(string serial);
    }
}
