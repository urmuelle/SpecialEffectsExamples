// <copyright file="ParticleEvent.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public abstract class ParticleEvent
    {
        private MinMax<float> timeRange;
        private float actualTime;
        private bool fade;
        private ParticleEvent nextFadeEvent;

        public ParticleEvent()
        {
            nextFadeEvent = null;
        }

        public ParticleEvent NextFadeEvent
        {
            get { return nextFadeEvent; }
            set { nextFadeEvent = value; }
        }

        public MinMax<float> TimeRange
        {
            get { return timeRange; }
            set { timeRange = value; }
        }

        public float ActualTime
        {
            get { return actualTime; }
            set { actualTime = value; }
        }

        public static void ProcessPropEqualsValue(ref MinMax<float> prop, ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            bool ended;

            // next token should be =
            ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }

            if (tokenIter.Current.Type != TokenType.Equals)
            {
                throw new Exception("Expecting = after property.");
            }

            // next token should be a number
            ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }

            prop = ParticleEmitterTokenizer.ProcessNumber(ref tokenIter);
        }

        public static void ProcessPropEqualsValue(ref MinMax<Vector3> prop, ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            bool ended;

            // next token should be =
            ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }

            if (tokenIter.Current.Type != TokenType.Equals)
            {
                throw new Exception("Expecting = after property.");
            }

            // next token should be a number
            ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }

            prop = ParticleEmitterTokenizer.ProcessVector(ref tokenIter);
        }

        public static void ProcessPropEqualsValue(ref MinMax<Color> prop, ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            bool ended;

            // next token should be =
            ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }

            if (tokenIter.Current.Type != TokenType.Equals)
            {
                throw new Exception("Expecting = after property.");
            }

            // next token should be a number
            ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }

            prop = ParticleEmitterTokenizer.ProcessColor(ref tokenIter);
        }

        public bool IsFade()
        {
            return fade;
        }

        public void SetFade(bool data = true)
        {
            fade = data;
        }

        public abstract void DoItToIt(ref Particle part);

        public abstract bool FadeAllowed();

        public abstract bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter);
    }
}
