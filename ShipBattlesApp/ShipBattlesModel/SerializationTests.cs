using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipBattlesModel
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestAIShipSerializtion()
        {
            Controller ctrl = new Controller();
            Location loc = ctrl.MakeRandLocation();
            Direction dir = ctrl.MakeRandDirection();
            AIShip ship = new AIShip() { Loc = loc, Direct = dir, Speed = 1 };
            string serial = ship.Serialize();
            Console.WriteLine(serial);
            Location newLoc = ctrl.MakeRandLocation();
            Direction newDir = ctrl.MakeRandDirection();
            AIShip newShip = new AIShip() { Loc = newLoc, Direct = newDir, Speed = 0 };
            newShip.Deserialize(serial);
            Console.WriteLine(newShip);
            Assert.IsTrue(newShip.Loc == loc);
            Assert.IsTrue(newShip.Speed == 1);
            Assert.IsTrue(newShip.Direct == dir);
        }

        [TestMethod]
        public void TestRepairKitSerializtion()
        {
            Controller ctrl = new Controller();
            Location loc = ctrl.MakeRandLocation();
            Direction dir = ctrl.MakeRandDirection();
            RepairKit kit = new RepairKit() { Loc = loc, Direct = dir, Speed = -1 };
            string serial = kit.Serialize();
            Location newLoc = ctrl.MakeRandLocation();
            Direction newDir = ctrl.MakeRandDirection();
            RepairKit newKit = new RepairKit() { Loc = newLoc, Direct = newDir, Speed = 0 };
            newKit.Deserialize(serial);
            Assert.IsTrue(newKit.Loc == loc);
            Assert.IsTrue(newKit.Speed == -1);
            Assert.IsTrue(newKit.Direct == dir);
        }

        [TestMethod]
        public void TestAsteroidSerialization()
        {
            Controller ctrl = new Controller();
            Location loc = ctrl.MakeRandLocation();
            Direction dir = ctrl.MakeRandDirection();
            Asteroid ast = new Asteroid() { Loc = loc, Direct = dir, Speed = -1 };
            string serial = ast.Serialize();
            Location newLoc = ctrl.MakeRandLocation();
            Direction newDir = ctrl.MakeRandDirection();
            Asteroid newAst = new Asteroid() { Loc = newLoc, Direct = newDir, Speed = 0 };
            newAst.Deserialize(serial);
            Assert.IsTrue(newAst.Loc == loc);
            Assert.IsTrue(newAst.Speed == -1);
            Assert.IsTrue(newAst.Direct == dir);
        }

        [TestMethod]
        public void TestBulletSerialization()
        {
            Controller ctrl = new Controller();
            Location loc = ctrl.MakeRandLocation();
            Direction dir = ctrl.MakeRandDirection();
            Bullet bull = new Bullet() { Loc = loc, Direct = dir, Speed = -1 };
            string serial = bull.Serialize();
            Location newLoc = ctrl.MakeRandLocation();
            Direction newDir = ctrl.MakeRandDirection();
            Bullet newBull = new Bullet() { Loc = newLoc, Direct = newDir, Speed = 0 };
            newBull.Deserialize(serial);
            Assert.IsTrue(newBull.Loc == loc);
            Assert.IsTrue(newBull.Speed == -1);
            Assert.IsTrue(newBull.Direct == dir);
        }

    }
}
