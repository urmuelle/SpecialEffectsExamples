namespace SimpleParticleSystemUnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SimpleParticleSystem.Particle_System;

    [TestClass]
    public class RecyclingArrayTests
    {
        [TestMethod]
        public void CreateNewArray()
        {
            var recyclingArray = new RecyclingArray<int>(10);

            Assert.IsNotNull(recyclingArray);
            Assert.AreEqual(10, recyclingArray.GetTotalElements());
        }

        [TestMethod]
        public void CreateNewParticleArray()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            Assert.IsNotNull(recyclingArray);
            Assert.AreEqual(10, recyclingArray.GetTotalElements());
        }

        [TestMethod]
        public void AddParticleToArray()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            var res = recyclingArray.New();

            Assert.IsNotNull(res);
            Assert.AreEqual(10, recyclingArray.GetTotalElements());
            Assert.AreEqual(1, recyclingArray.GetNumUsedElements());
            Assert.AreEqual(9, recyclingArray.GetNumFreeElements());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddParticleToFullArrayNotPossible()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            for (var i = 0; i < 10; i++)
            {
                var res = recyclingArray.New();
            }

            Assert.AreEqual(10, recyclingArray.GetTotalElements());
            Assert.AreEqual(10, recyclingArray.GetNumUsedElements());
            Assert.AreEqual(0, recyclingArray.GetNumFreeElements());

            var res2 = recyclingArray.New();
        }

        [TestMethod]        
        public void DeleteAllFromArray()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            for (var i = 0; i < 10; i++)
            {
                var res = recyclingArray.New();
            }

            recyclingArray.DeleteAll();

            Assert.AreEqual(10, recyclingArray.GetTotalElements());
            Assert.AreEqual(0, recyclingArray.GetNumUsedElements());
            Assert.AreEqual(10, recyclingArray.GetNumFreeElements());            
        }

        [TestMethod]
        public void DeleteElementFromArrayWithIndex()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            for (var i = 0; i < 10; i++)
            {
                var res = recyclingArray.New();
            }

            recyclingArray.Delete(0);

            Assert.AreEqual(10, recyclingArray.GetTotalElements());
            Assert.AreEqual(9, recyclingArray.GetNumUsedElements());
            Assert.AreEqual(1, recyclingArray.GetNumFreeElements());
        }

        [TestMethod]
        public void DeleteElementFromArrayWithHandle()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            Particle res = null;

            for (var i = 0; i < 10; i++)
            {
                res = recyclingArray.New();
            }

            recyclingArray.Delete(res);

            Assert.AreEqual(10, recyclingArray.GetTotalElements());
            Assert.AreEqual(9, recyclingArray.GetNumUsedElements());
            Assert.AreEqual(1, recyclingArray.GetNumFreeElements());
        }

        [TestMethod]
        public void DeleteElementFromArrayAndAddNew()
        {
            var recyclingArray = new RecyclingArray<Particle>(10);

            Particle res = null;

            for (var i = 0; i < 10; i++)
            {
                res = recyclingArray.New();
            }

            recyclingArray.Delete(res);

            res = recyclingArray.New();

            Assert.AreEqual(10, recyclingArray.GetTotalElements());
            Assert.AreEqual(10, recyclingArray.GetNumUsedElements());
            Assert.AreEqual(0, recyclingArray.GetNumFreeElements());
        }
    }
}