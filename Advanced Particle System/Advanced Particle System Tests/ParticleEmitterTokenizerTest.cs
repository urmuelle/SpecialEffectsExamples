using System;
using System.Collections.Generic;
using AdvancedParticleSystem.ParticleSystem.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedParticleSystemTests
{
    [TestClass]
    public class ParticleEmitterTokenizerTest
    {
        [TestMethod]
        public void CreateParticleTokenizerTest()
        {
            var tokenizer = new ParticleEmitterTokenizer();
            Assert.IsNotNull(tokenizer);
        }

        [TestMethod]
        public void ProcessColorTokenizerTest()
        {
            List<ParticleEmitterToken> colorTokens = new List<ParticleEmitterToken>
            {
                // rgba(random(0, 1), 0, 1, 1)
                new ParticleEmitterToken() { StringValue = "rgba", Type = TokenType.KeywordColor },
                new ParticleEmitterToken() { StringValue = "(", Type = TokenType.OpenParen },
                new ParticleEmitterToken() { StringValue = "random", Type = TokenType.KeywordRandom },
                new ParticleEmitterToken() { StringValue = "(", Type = TokenType.OpenParen },
                new ParticleEmitterToken() { StringValue = "0", Type = TokenType.RealNumber },
                new ParticleEmitterToken() { StringValue = ",", Type = TokenType.Comma },
                new ParticleEmitterToken() { StringValue = "1", Type = TokenType.RealNumber },
                new ParticleEmitterToken() { StringValue = ")", Type = TokenType.CloseParen },
                new ParticleEmitterToken() { StringValue = ",", Type = TokenType.Comma },
                new ParticleEmitterToken() { StringValue = "0", Type = TokenType.RealNumber },
                new ParticleEmitterToken() { StringValue = ",", Type = TokenType.Comma },
                new ParticleEmitterToken() { StringValue = "1", Type = TokenType.RealNumber },
                new ParticleEmitterToken() { StringValue = ",", Type = TokenType.Comma },
                new ParticleEmitterToken() { StringValue = "1", Type = TokenType.RealNumber },
                new ParticleEmitterToken() { StringValue = ")", Type = TokenType.CloseParen },
                // Add a random next token as in correct grammar, this statement alone would not be valid
                new ParticleEmitterToken() { StringValue = ")", Type = TokenType.CloseBrace }
            };

            var tokenIter = colorTokens.GetEnumerator();
            tokenIter.MoveNext();
            var c = ParticleEmitterTokenizer.ProcessColor(ref tokenIter);
            Assert.AreEqual(0, c.Max.G);
            Assert.AreEqual(255, c.Max.B);
            Assert.AreEqual(255, c.Max.A);
        }
    }
}
