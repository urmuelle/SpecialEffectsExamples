// <copyright file="ParticleEventVelocityZ.cs" company="Urs Müller">
// </copyright>

namespace AdvancedParticleSystem.ParticleSystem.Parser
{
    using SpecialEffectsExamplesLibrary;
    using System;
    using System.Collections.Generic;

    public class ParticleEventVelocityZ : ParticleEvent
    {
        private MinMax<float> velocityZ;

        public MinMax<float> VelocityZ
        {
            get { return velocityZ; }
            set { velocityZ = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var direction = part.Direction;
                direction.Z = velocityZ.GetRandomNumInRange();
                part.Direction = direction;
            }

            if (NextFadeEvent != null)
            {
                var newVelocityZ = ((ParticleEventVelocityZ)NextFadeEvent).VelocityZ.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var dirStep = part.DirectionStep;

                // calculate color deltas to get us to the next fade event.
                dirStep.Z = newVelocityZ - (part.Direction.Z / timedelta);

                part.DirectionStep = dirStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("VELOCITYZ") == false)
            {
                throw new Exception("Expecting Velocity Z!");
            }

            ProcessPropEqualsValue(ref velocityZ, ref tokenIter);

            return true;
        }
    }
}
