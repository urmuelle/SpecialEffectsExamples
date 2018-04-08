// <copyright file="MinMax.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MinMax<T>
    {
        private T min;
        private T max;
        private Helper helper;

        public MinMax()
        {
            this.min = default;
            this.max = default;

            helper = new Helper();
        }

        public MinMax(T min, T max)
        {
            this.min = min;
            this.max = max;

            helper = new Helper();
        }

        public T Min
        {
            get { return min; }
            set { min = value; }
        }

        public T Max
        {
            get { return max; }
            set { max = value; }
        }

        public T GetRandomNumInRange()
        {
            dynamic arg1 = (dynamic)min;
            dynamic arg2 = (dynamic)max;
            dynamic ret;

            try
            {
                ret = helper.RandomNumber(arg1, arg2);
                return ret;
            }
            catch
            {
                throw new Exception("Type does not support numeric cast");
            }
        }

        public T GetRange()
        {
            dynamic arg1 = (dynamic)min;
            dynamic arg2 = (dynamic)max;
            dynamic ret;

            try
            {
                ret = arg1 - arg2;
                return Math.Abs(ret);
            }
            catch
            {
                throw new Exception("Type does not support subtraction");
            }
        }
    }
}
