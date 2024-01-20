using System;
using System.Collections.Generic;
using AdvancedParticleSystemMG38_1.ParticleSystem.Parser;

namespace AdvancedParticleSystemTests
{
    public class ParticleEmitterTokenizerTest
    {
        [Test]
        public void CreateParticleTokenizerTest()
        {
            var tokenizer = new ParticleEmitterTokenizer();
            Assert.That(tokenizer, Is.Not.Null);
        }

        [Test]
        public void ProcessColorTokenizerTest()
        {
            List<ParticleEmitterToken> colorTokens = new()
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
            Assert.Multiple(() =>
            {
                Assert.That(c.Max.G, Is.EqualTo(0));
                Assert.That(c.Max.B, Is.EqualTo(255));
                Assert.That(c.Max.A, Is.EqualTo(255));
            });
        }
    }
}
