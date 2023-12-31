// <copyright file="RecyclingArray.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SimpleParticleSystemMG38_1.ParticleSystem
{
    using System;

    public class RecyclingArray<T>
        where T : new()
    {
        private int usedElements;
        private T[] elements;
        private bool[] alive;
        private int lastNew;

        private int numElements;

        public RecyclingArray(int numElements)
        {
            this.numElements = numElements;
            elements = new T[numElements];
            alive = new bool[numElements];

            for (int q = 0; q < numElements; q++)
            {
                alive[q] = false;
            }

            usedElements = 0;
            lastNew = 0;
        }

        public T New()
        {
            // assert that we have space for this one
            if (GetNumFreeElements() < 1)
            {
                throw new ArgumentOutOfRangeException("RecyclingArray.New: too many objects!");
            }

            // find first element not in use.  as an optimization, we start at
            // the position that was allocated last, in the hopes that the next position
            // will be free.
            int i = lastNew;

            for (int q = 0; q < numElements; q++)
            {
                if (!alive[i])
                {
                    // we've found our free spot!  use it!
                    break;
                }
                else
                {
                    i++;
                    if (i >= numElements)
                    {
                        i = 0;
                    }
                }
            }

            if (alive[i])
            {
                // huh? if we got here, there are no free elements in the list... yet
                // GetNumFreeElements didn't tell us that in the beginning.  Logic error.
                throw new Exception("ArrayElement.New(): internal logic error.");
            }

            if (elements[i] == null)
            {
                elements[i] = new T();
            }

            // increment used count
            usedElements++;
            alive[i] = true;
            lastNew = i;

            // return it
            return elements[i];
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= numElements || !alive[index])
            {
                return false;
            }

            // don't actually delete element at index;
            // just mark it free.
            alive[index] = false;
            usedElements--;

            return true;
        }

        public bool Delete(T elem)
        {
            if (usedElements == 0)
            {
                return false;
            }

            var index = -1;

            for (int q = 0; q < numElements; q++)
            {
                if (elem.Equals(elements[q]))
                {
                    index = q;
                }
            }

            if (index < 0 || index >= numElements || !alive[index])
            {
                return false;
            }

            alive[index] = false;
            usedElements--;

            return true;
        }

        public int GetNumFreeElements()
        {
            return numElements - GetNumUsedElements();
        }

        public int GetNumUsedElements()
        {
            return usedElements;
        }

        public int GetTotalElements()
        {
            return numElements;
        }

        public T GetAt(int index)
        {
            return elements[index];
        }

        public bool IsAlive(int index)
        {
            return alive[index];
        }

        public void DeleteAll()
        {
            for (int q = 0; q < numElements; q++)
            {
                alive[q] = false;
            }

            usedElements = 0;
            lastNew = 0;
        }
    }
}
