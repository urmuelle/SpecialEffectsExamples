﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdvancedParticleSystem.Parser;

namespace AdvancedParticleSystemTests
{
    [TestClass]
    public class TokenizerTests
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
        public void CreateTokenizerTest()
        {
            var tokenizer = new ParticleEmitterTokenizer();
            Assert.IsNotNull(tokenizer);
        }

        [TestMethod]
        public void TokenizeTestString()
        {
            var tokenizer = new ParticleEmitterTokenizer();
            tokenizer.Tokenize(testString);
            Assert.AreEqual(96, tokenizer.TokenVector.Count);
        }
    }
}
