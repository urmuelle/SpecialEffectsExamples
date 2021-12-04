// <copyright file="ParticleEventTimer.cs" company="Urs Müller">
// </copyright>

namespace AdvancedParticleSystemMG38.ParticleSystem.Parser
{
    using SpecialEffectsExamplesLibrary;
    using System;
    using System.Collections.Generic;

    public class ParticleEventTimer : ParticleEvent
    {
        private MinMax<float> eventTimer;

        public MinMax<float> EventTimer
        {
            get { return eventTimer; }
            set { eventTimer = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            part.EventTimer = eventTimer.GetRandomNumInRange();
        }

        public override bool FadeAllowed()
        {
            return false;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("EVENTTIMER") == false)
            {
                throw new Exception("Expecting Event Timer!");
            }

            ProcessPropEqualsValue(ref eventTimer, ref tokenIter);

            return true;
        }
    }
}
