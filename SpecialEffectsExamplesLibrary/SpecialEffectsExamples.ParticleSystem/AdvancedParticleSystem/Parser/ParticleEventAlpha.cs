// <copyright file="ParticleEventAlpha.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;

    public class ParticleEventAlpha : ParticleEvent
    {
        private MinMax<float> alpha;

        public MinMax<float> Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var color = part.Color;
                color.A = (byte)Alpha.GetRandomNumInRange();
                part.Color = color;
            }

            if (NextFadeEvent != null)
            {
                var newAlphaValue = ((ParticleEventAlpha)NextFadeEvent).Alpha.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var colorStep = part.ColorStep;

                // calculate color deltas to get us to the next fade event.
                colorStep.A = (byte)(newAlphaValue - (part.Color.A / timedelta));

                part.ColorStep = colorStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("ALPHA") == false)
            {
                throw new Exception("Expecting Size!");
            }

            ProcessPropEqualsValue(ref alpha, ref tokenIter);

            return true;
        }
    }
}
