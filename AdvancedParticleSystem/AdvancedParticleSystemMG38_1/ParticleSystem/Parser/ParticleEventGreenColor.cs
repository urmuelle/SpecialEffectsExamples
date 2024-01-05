// <copyright file="ParticleEventGreenColor.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystemMG38_1.ParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEventGreenColor : ParticleEvent
    {
        private MinMax<float> greenColor;

        public MinMax<float> GreenColor
        {
            get { return greenColor; }
            set { greenColor = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var color = part.Color;
                color.G = (byte)GreenColor.GetRandomNumInRange();
                part.Color = color;
            }

            if (NextFadeEvent != null)
            {
                var newGreenValue = ((ParticleEventGreenColor)NextFadeEvent).GreenColor.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var colorStep = part.ColorStep;

                // calculate color deltas to get us to the next fade event.
                colorStep.G = (byte)(newGreenValue - (part.Color.G / timedelta));

                part.ColorStep = colorStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("GREENCOLOR") == false)
            {
                throw new Exception("Expecting Green Color!");
            }

            ProcessPropEqualsValue(ref greenColor, ref tokenIter);

            return true;
        }
    }
}
