using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genetic_Pathfinder;

namespace PathfinderUnitTestProject
{
    [TestClass]
    public class EngineTests
    {
        [TestMethod]
        public void TargetLocationValid_50_50_trueReturned()   
        {
            int x = 50;
            int y = 50;
            bool expected = true;
            bool actual = Engine.TargetLocationVaild(x, y);
            Assert.AreEqual(expected, actual);
        }
    }
}
