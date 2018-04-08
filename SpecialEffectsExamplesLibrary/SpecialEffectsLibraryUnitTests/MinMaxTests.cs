// <copyright file="MinMaxTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpecialEffectsLibraryUnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SpecialEffectsExamplesLibrary;

    [TestClass]
    public class MinMaxTests
    {
        [TestMethod]
        public void CreateDefaultMinMaxType()
        {
            var result = new MinMax<int>();

            Assert.AreEqual(0, result.Min);
            Assert.AreEqual(0, result.Max);
        }

        [TestMethod]
        public void CreateMinMaxType()
        {
            var result = new MinMax<int>(-1, 3);

            Assert.AreEqual(-1, result.Min);
            Assert.AreEqual(3, result.Max);
        }

        [TestMethod]
        public void GetMinMaxRange()
        {
            var result = new MinMax<int>(-1, 3);

            Assert.AreEqual(4, result.GetRange());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetMinMaxRangeNonNumericThrowsException()
        {
            var result = new MinMax<object>(null, null);

            var res = result.GetRange();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetMinMaxRandomNumberNonNumericThrowsException()
        {
            var result = new MinMax<object>(null, null);

            var res = result.GetRandomNumInRange();
        }
    }
}
