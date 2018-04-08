// <copyright file="StringExtensions.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class StringExtensions
    {
        public static string RemoveQuotes(this string str)
        {
            return str.Replace("\"", string.Empty);
        }
    }
}
