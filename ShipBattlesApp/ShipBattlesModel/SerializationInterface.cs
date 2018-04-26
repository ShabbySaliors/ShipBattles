﻿//----------------------------------------------------------
//File:   SerializationInterface.cs
//Desc:   This file contains an implementable interface that
//        all serializable objects in ShipBattles can use to
//        save/load.
//----------------------------------------------------------
namespace ShipBattlesModel
{
    /// <summary>
    /// Serialization interface for all serializable objects
    /// </summary>
    interface ISerializible
    {
        /// <summary>
        /// Undefined interface method for serialization
        /// of in-game objects
        /// </summary>
        /// <returns>Comma-delineated string</returns>
        string Serialize();

        /// <summary>
        /// Undefined interface method for deserialization
        /// of in-game objects
        /// </summary>
        /// <param name="serial">
        /// String of object attributes formatted by
        /// `ISerializable.Serialize()`
        /// </param>
        void Deserialize(string serial);
    }
}
