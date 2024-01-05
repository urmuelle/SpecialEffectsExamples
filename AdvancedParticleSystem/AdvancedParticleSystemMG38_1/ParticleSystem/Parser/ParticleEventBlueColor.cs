// <copyright file="ParticleEventBlueColor.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystemMG38_1.ParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEventBlueColor : ParticleEvent
    {
        private MinMax<float> blueColor;

        public MinMax<float> BlueColor
        {
            get { return blueColor; }
            set { blueColor = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                var color = part.Color;
                color.B = (byte)BlueColor.GetRandomNumInRange();
                part.Color = color;
            }

            if (NextFadeEvent != null)
            {
                var newBlueValue = ((ParticleEventBlueColor)NextFadeEvent).BlueColor.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var colorStep = part.ColorStep;

                // calculate color deltas to get us to the next fade event.
                colorStep.B = (byte)(newBlueValue - (part.Color.B / timedelta));

                part.ColorStep = colorStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("BLUECOLOR") == false)
            {
                throw new Exception("Expecting Blue Color!");
            }

            ProcessPropEqualsValue(ref blueColor, ref tokenIter);

            return true;
        }
    }
}
