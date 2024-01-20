using System;
using AdvancedParticleSystemMG38_1.ParticleSystem.Parser;

namespace AdvancedParticleSystemTests
{
    public class TokenTest
    {
        [Test]
        public void CreateToken()
        {
            var token = new ParticleEmitterToken();
            Assert.That(token, Is.Not.Null);
        }

        [Test]
        public void GetUnknownToken()
        {
            var token = new ParticleEmitterToken();
            Assert.That(token.TypeAsString(), Is.EqualTo("unknown"));
        }
    }
}
