// <copyright file="ParticleEmitterToken.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum TokenType
    {
        RealNumber = 1,
        Equals, Comma, OpenParen, CloseParen, OpenBrace, CloseBrace, Quote,

        KeywordParticleSystem, KeywordEventSequence,
        KeywordRandom, KeywordXYZ, KeywordColor, KeywordTexture, AlphaBlendMode,

        // system property tokens
        SysVectorProp, SeqAlphaBlendModeProp,

        // event time tokens
        KeywordFade, KeywordSo, KeywordAt, KeywordInitial, KeywordFinal,

        // particle properties (event types)
        ParticleNumericProp, ParticleVectorProp, ParticleColorProp,

        // event sequence props
        SeqNumericProp, SeqVectorProp,

        Unknown
    }

    public class ParticleEmitterToken
    {
        private string stringValue;
        private TokenType type;

        public TokenType Type
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
            }
        }

        public string StringValue
        {
            get
            {
                return stringValue;
            }

            set
            {
                this.stringValue = value;
            }
        }

        public bool IsEmitRate()
        {
            return stringValue.Contains("EMITRATE");
        }

        public bool IsPosition()
        {
            return stringValue.Contains("POSITION");
        }

        public bool IsSpawnDir()
        {
            return stringValue.Contains("SPAWNDIR");
        }

        public bool IsEmitRadius()
        {
            return stringValue.Contains("EMITRADIUS");
        }

        public bool IsLifeTime()
        {
            return stringValue.Contains("LIFETIME");
        }

        public bool IsNumParticles()
        {
            return stringValue.Contains("NUMPARTICLES");
        }

        public bool IsLoops()
        {
            return stringValue.Contains("LOOPS");
        }

        public bool IsGravity()
        {
            return stringValue.Contains("GRAVITY");
        }

        public bool IsSrcBlendMode()
        {
            return stringValue.Contains("SOURCEBLENDMODE");
        }

        public bool IsDestBlendMode()
        {
            return stringValue.Contains("DESTBLENDMODE");
        }

        public string TypeAsString()
        {
            switch (type)
            {
                case TokenType.RealNumber: return "RealNumber";
                case TokenType.Equals: return "Equals";
                case TokenType.Comma: return "Comma";
                case TokenType.OpenParen: return "OpenParen";
                case TokenType.CloseParen: return "CloseParen";
                case TokenType.OpenBrace: return "OpenBrace";
                case TokenType.CloseBrace: return "CloseBrace";
                case TokenType.Quote: return "Quote";

                case TokenType.KeywordParticleSystem: return "KeywordParticleSystem";
                case TokenType.KeywordEventSequence: return "KeywordEventSequence";
                case TokenType.KeywordRandom: return "KeywordRandom";
                case TokenType.KeywordXYZ: return "KeywordXYZ";
                case TokenType.KeywordColor: return "KeywordColor";
                case TokenType.KeywordTexture: return "KeywordTexture";

                case TokenType.SysVectorProp: return "SysVectorProp";
                case TokenType.SeqAlphaBlendModeProp: return "SeqAlphaBlendModeProp";
                case TokenType.AlphaBlendMode: return "AlphaBlendMode";

                case TokenType.KeywordFade: return "KeywordFade";
                case TokenType.KeywordSo: return "KeywordSo";
                case TokenType.KeywordAt: return "KeywordAt";
                case TokenType.KeywordInitial: return "KeywordInitial";
                case TokenType.KeywordFinal: return "KeywordFinal";

                case TokenType.ParticleNumericProp: return "ParticleNumericProp";
                case TokenType.ParticleVectorProp: return "ParticleVectorProp";
                case TokenType.ParticleColorProp: return "ParticleColorProp";

                case TokenType.SeqNumericProp: return "SeqNumericProp";
                case TokenType.SeqVectorProp: return "SeqVectorProp";
            }

            return "unknown";
        }
    }
}
