// <copyright file="ParticleEventSize.cs" company="Urs Müller">
// </copyright>

namespace AdvancedParticleSystem.ParticleSystem.Parser
{
    using SpecialEffectsExamplesLibrary;
    using System;
    using System.Collections.Generic;

    public class ParticleEventSize : ParticleEvent
    {
        private MinMax<float> size;

        public MinMax<float> Size
        {
            get { return size; }
            set { size = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                part.Size = size.GetRandomNumInRange();
            }

            if (this.NextFadeEvent != null)
            {
                float newvalue = ((ParticleEventSize)NextFadeEvent).Size.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                part.SizeStep = newvalue - (part.Size / timedelta);
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("SIZE") == false)
            {
                throw new Exception("Expecting Size!");
            }

            ProcessPropEqualsValue(ref size, ref tokenIter);

            return true;
        }
    }
}
