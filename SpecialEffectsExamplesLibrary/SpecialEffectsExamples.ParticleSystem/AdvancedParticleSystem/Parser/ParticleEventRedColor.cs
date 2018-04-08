// <copyright file="ParticleEventRedColor.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class ParticleEventRedColor : ParticleEvent
    {
        private MinMax<float> redColor;

        public MinMax<float> RedColor
        {
            get { return redColor; }
            set { redColor = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var color = part.Color;
                color.R = (byte)RedColor.GetRandomNumInRange();
                part.Color = color;
            }

            if (NextFadeEvent != null)
            {
                var newRedValue = ((ParticleEventRedColor)NextFadeEvent).RedColor.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var colorStep = part.ColorStep;

                // calculate color deltas to get us to the next fade event.
                colorStep.R = (byte)(newRedValue - (part.Color.R / timedelta));

                part.ColorStep = colorStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("REDCOLOR") == false)
            {
                throw new Exception("Red Color!");
            }

            ProcessPropEqualsValue(ref redColor, ref tokenIter);

            return true;
        }
    }
}
