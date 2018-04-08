using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace AdvancedParticleSystemTests
{
    [TestClass]
    public class ParticleEmitterTests
    {
        private const string testString =
            @"particlesystem ""test"" 1.00 {
              position = XYZ(0,1,0)
              eventsequence ""test"" {
                sourceblendmode = D3DBLEND_ONE
                destblendmode = D3DBLEND_SRCALPHA
                numparticles = 50
                gravity = XYZ(0,-1,0)
                emitrate = 20
                lifetime = Random(1,2)
                texture = ""Ch19p1_ParticleTexture.png""
                initial color = rgba(random(0, 1), 0, 1, 1)
                initial velocity = XYZ(random(-3, 3), random(1, 5), random(-5, 5))
              }
            }";

        [TestMethod]
        public void CreateParticleEmitter()
        {
            var emitter = new AdvancedParticleSystem.ParticleSystem.ParticleEmitter(null, null);
            emitter.Compile(testString);
            Assert.AreEqual(new Vector3(0, 1, 0), emitter.Position);
        }
    }
}
