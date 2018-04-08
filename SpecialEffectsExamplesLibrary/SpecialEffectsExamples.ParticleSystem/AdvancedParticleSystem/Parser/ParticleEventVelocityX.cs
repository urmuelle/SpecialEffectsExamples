// <copyright file="ParticleEventVelocityX.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;

    public class ParticleEventVelocityX : ParticleEvent
    {
        private MinMax<float> velocityX;

        public MinMax<float> VelocityX
        {
            get { return velocityX; }
            set { velocityX = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var direction = part.Direction;
                direction.X = velocityX.GetRandomNumInRange();
                part.Direction = direction;
            }

            if (NextFadeEvent != null)
            {
                var newVelocityX = ((ParticleEventVelocityX)NextFadeEvent).VelocityX.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var dirStep = part.DirectionStep;

                // calculate color deltas to get us to the next fade event.
                dirStep.X = newVelocityX - (part.Direction.X / timedelta);

                part.DirectionStep = dirStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("VELOCITYX") == false)
            {
                throw new Exception("Expecting Velocity X!");
            }

            ProcessPropEqualsValue(ref velocityX, ref tokenIter);

            return true;
        }
    }
}
