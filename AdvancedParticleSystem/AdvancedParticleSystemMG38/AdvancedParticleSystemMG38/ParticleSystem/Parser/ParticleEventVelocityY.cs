// <copyright file="ParticleEventVelocityY.cs" company="Urs Müller">
// </copyright>

namespace AdvancedParticleSystem.ParticleSystem.Parser
{
    using SpecialEffectsExamplesLibrary;
    using System;
    using System.Collections.Generic;

    public class ParticleEventVelocityY : ParticleEvent
    {
        private MinMax<float> velocityY;

        public MinMax<float> VelocityY
        {
            get { return velocityY; }
            set { velocityY = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var direction = part.Direction;
                direction.Y = velocityY.GetRandomNumInRange();
                part.Direction = direction;
            }

            if (NextFadeEvent != null)
            {
                var newVelocityY = ((ParticleEventVelocityY)NextFadeEvent).VelocityY.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var dirStep = part.DirectionStep;

                // calculate color deltas to get us to the next fade event.
                dirStep.Y = newVelocityY - (part.Direction.Y / timedelta);

                part.DirectionStep = dirStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("VELOCITYY") == false)
            {
                throw new Exception("Expecting Velocity Y!");
            }

            ProcessPropEqualsValue(ref velocityY, ref tokenIter);

            return true;
        }
    }
}
