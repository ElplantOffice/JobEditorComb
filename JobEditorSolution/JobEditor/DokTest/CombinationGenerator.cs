using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public static class CombinationGenerator
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> elements, int size)
        {
            if (size == 0)
            {
                yield return new T[0];
            }
            else
            {
                int i = 0;
                foreach (var element in elements)
                {
                    foreach (var comb in Combinations(elements.Skip(i + 1), size - 1))
                    {
                        yield return new T[] { element }.Concat(comb);
                    }
                    i++;
                }
            }
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> elements, int minSize, int maxSize)
        {
            IEnumerable<IEnumerable<T>> toReturn = new List<IEnumerable<T>>();
            for (int i = minSize; i <= maxSize; i++)
            {
                toReturn = toReturn.Concat(Combinations(elements, i));
            }
            return toReturn;
        }
    }
}
