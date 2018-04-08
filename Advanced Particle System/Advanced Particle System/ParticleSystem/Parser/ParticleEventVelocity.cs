// <copyright file="ParticleEventVelocity.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystem.ParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEventVelocity : ParticleEvent
    {
        private MinMax<Vector3> velocity;

        public MinMax<Vector3> Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                part.Direction = velocity.GetRandomNumInRange();
            }

            if (NextFadeEvent != null)
            {
                var newVelocity = ((ParticleEventVelocity)NextFadeEvent).Velocity.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var dirStep = part.DirectionStep;

                // calculate color deltas to get us to the next fade event.
                dirStep.X = newVelocity.X - (part.Direction.X / timedelta);
                dirStep.Y = newVelocity.Y - (part.Direction.Y / timedelta);
                dirStep.Z = newVelocity.Z - (part.Direction.Z / timedelta);

                part.DirectionStep = dirStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("VELOCITY") == false)
            {
                throw new Exception("Expecting Velocity!");
            }

            ProcessPropEqualsValue(ref velocity, ref tokenIter);

            return true;
        }
    }
}
