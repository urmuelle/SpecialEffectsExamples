// <copyright file="Helper.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SimpleParticleSystemMG38.Helper
{
    using System;

    public class Helper
    {
        public const int RandMax = 32767;

        private Random random;

        public Helper()
        {
            random = new Random();
        }

        public static int RandomNumber(int iMin, int iMax)
        {
            if (iMin == iMax)
            {
                return iMin;
            }

            var random = new Random();
            return (random.Next(RandMax) % (Math.Abs(iMax - iMin) + 1)) + iMin;
        }

        public float RandomNumber(float fMin, float fMax)
        {
            if (fMin == fMax)
            {
                return fMin;
            }

            float fRandom = (float)random.Next(RandMax) / (float)RandMax;
            return (fRandom * Math.Abs(fMax - fMin)) + fMin;
        }
    }
}
