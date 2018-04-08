using System;
using AdvancedParticleSystem.ParticleSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedParticleSystemTests
{
    [TestClass]
    public class MinMaxTests
    {
        [TestMethod]
        public void CreateMinMaxTypeTest()
        {
            MinMax<int> x = new MinMax<int>();
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public void CreateMinMaxTypeWithMinMaxTest()
        {
            MinMax<float> x = new MinMax<float>(0.0f, 10.0f);
            var diff = x.GetRange();
            Assert.AreEqual(10.0f, diff);
            var rand1 = x.GetRandomNumInRange();
            var rand2 = x.GetRandomNumInRange();

            Assert.AreNotEqual(rand1, rand2);
        }
    }
}
