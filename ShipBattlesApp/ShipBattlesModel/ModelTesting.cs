//---------------------------------------------
//File:   HighScoreTesting.cs
//Desc:   This file contains code to test the
//        capabilites of the game model.
//---------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipBattlesModel
{
    [TestClass]
    public class ModelTesting
    {
        [TestMethod]
        public void TestLoadWorld()
        {
            Controller ctrl = new Controller();
            ctrl.LoadWorld(1);
            Assert.IsTrue(GameWorld.Instance.Objects.Count == 17);
            Assert.IsTrue(GameWorld.Instance.Height == 300);
            Assert.IsTrue(GameWorld.Instance.Width == 300);
        }

        [TestMethod]
        public void TestMakeRandDirection()
        {
            Controller ctrl = new Controller();
            for(int i = 0; i < 20; i++)
            {
                Direction dir = ctrl.MakeRandDirection();
                Assert.IsTrue(dir.Up <= 1 && dir.Right <= 1);
                Assert.IsTrue(dir.Up >= -1 && dir.Right >= -1);
            }
        }

        [TestMethod]
        public void TestMakeRandLocation()
        {
            Controller ctrl = new Controller();
            ctrl.LoadWorld(1);
            for (int i = 0; i < 200; i++)
            {
                Location loc = ctrl.MakeRandLocation();
                Assert.IsTrue(loc.X <= GameWorld.Instance.Width && loc.X >= 0);
                Assert.IsTrue(loc.Y <= GameWorld.Instance.Height && loc.Y >= 0);
            }
        }
    }
}
