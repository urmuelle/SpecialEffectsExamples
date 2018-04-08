﻿// <copyright file="NumericExtensions.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary
{
    using System;

    /// <summary>
    /// Convert to Radians.
    /// </summary>
    /// <param name="val">The value to convert to radians</param>
    /// <returns>The value in radians</returns>
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
