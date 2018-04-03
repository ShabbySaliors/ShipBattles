﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ctrl.LoadWorld();
            Assert.IsTrue(ctrl.World.Objects.Count == 17);
            Assert.IsTrue(ctrl.World.Height == 300);
            Assert.IsTrue(ctrl.World.Width == 300);
        }

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

        public void TestMakeRandLocation()
        {
            Controller ctrl = new Controller();
            ctrl.LoadWorld();
            for (int i = 0; i < 200; i++)
            {
                Location loc = ctrl.MakeRandLocation();
                Assert.IsTrue(loc.X <= ctrl.World.Width && loc.X >= 0);
                Assert.IsTrue(loc.Y <= ctrl.World.Height && loc.Y >= 0);
            }
        }
    }
}
