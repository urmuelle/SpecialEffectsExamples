using System;
using AdvancedParticleSystem.ParticleSystem.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedParticleSystemTests
{
    [TestClass]
    public class TokenTest
    {
        [TestMethod]
        public void CreateToken()
        {
            var token = new ParticleEmitterToken();
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void GetUnknownToken()
        {
            var token = new ParticleEmitterToken();
            Assert.AreEqual("unknown", token.TypeAsString());
        }
    }
}
