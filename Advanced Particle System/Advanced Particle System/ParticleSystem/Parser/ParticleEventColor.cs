// <copyright file="ParticleEventColor.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystem.ParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEventColor : ParticleEvent
    {
        private MinMax<Color> color;

        public MinMax<Color> Color
        {
            get { return color; }
            set { color = value; }
        }

        public override void DoItToIt(ref Particle part)
        {
            if (!IsFade())
            {
                part.Color = Color.GetRandomNumInRange();
            }

            if (NextFadeEvent != null)
            {
                var newColor = ((ParticleEventColor)NextFadeEvent).Color.GetRandomNumInRange();
                float timedelta = NextFadeEvent.ActualTime - ActualTime;

                if (timedelta == 0)
                {
                    timedelta = 1; // prevent divide by zero errors
                }

                var colorStep = part.ColorStep;

                // calculate color deltas to get us to the next fade event.
                colorStep.R = (byte)(newColor.R - (part.Color.R / timedelta));
                colorStep.G = (byte)(newColor.G - (part.Color.G / timedelta));
                colorStep.B = (byte)(newColor.B - (part.Color.B / timedelta));
                colorStep.A = (byte)(newColor.A - (part.Color.A / timedelta));

                part.ColorStep = colorStep;
            }
        }

        public override bool FadeAllowed()
        {
            return true;
        }

        public override bool ProcessTokenStream(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.StringValue.Contains("COLOR") == false)
            {
                throw new Exception("Expecting Color!");
            }

            ProcessPropEqualsValue(ref color, ref tokenIter);

            return true;
        }
    }
}
