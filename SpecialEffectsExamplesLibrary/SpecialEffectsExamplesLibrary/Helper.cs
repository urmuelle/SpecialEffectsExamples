// <copyright file="Helper.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary
{
    using System;
    using Microsoft.Xna.Framework;

    public class Helper
    {
        public const int RandMax = 32767;

        private Random random;

        public Helper()
        {
            random = new Random();
        }

        public static int RandomNumber(int min, int max)
        {
            if (min == max)
            {
                return min;
            }

            var random = new Random();
            return (random.Next(RandMax) % (Math.Abs(max - min) + 1)) + min;
        }

        public float RandomNumber(float min, float max)
        {
            if (min == max)
            {
                return min;
            }

            float fRandom = (float)random.Next(RandMax) / (float)RandMax;
            return (fRandom * Math.Abs(max - min)) + min;
        }

        public Vector3 RandomNumber(Vector3 min, Vector3 max)
        {
            if (min.X == max.X && min.Y == max.Y && min.Z == max.Z)
            {
                return min;
            }

            float fRandomX = (float)random.Next(RandMax) / (float)RandMax;
            float fRandomY = (float)random.Next(RandMax) / (float)RandMax;
            float fRandomZ = (float)random.Next(RandMax) / (float)RandMax;

            return Vector3.Multiply(
                new Vector3(fRandomX, fRandomY, fRandomZ),
                new Vector3(Math.Abs(max.X - min.X), Math.Abs(max.Y - min.Y), Math.Abs(max.Z - min.Z))) + min;
        }

        public Color RandomNumber(Color min, Color max)
        {
            if (min.R == max.R && min.G == max.G && min.B == max.B && min.A == max.A)
            {
                return min;
            }

            float randomR = (float)random.Next(RandMax) / (float)RandMax;
            float randomG = (float)random.Next(RandMax) / (float)RandMax;
            float randomB = (float)random.Next(RandMax) / (float)RandMax;
            float randomA = (float)random.Next(RandMax) / (float)RandMax;

            int finalR = (int)(randomR * Math.Abs(max.R - min.R)) + min.R;
            int finalG = (int)(randomG * Math.Abs(max.G - min.G)) + min.G;
            int finalB = (int)(randomB * Math.Abs(max.B - min.B)) + min.B;
            int finalA = (int)(randomA * Math.Abs(max.A - min.A)) + min.A;

            var col = new Color(finalR, finalG, finalB, finalA);
            return col;
        }
    }
}
