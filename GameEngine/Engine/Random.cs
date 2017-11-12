using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class Random
    {

        private static System.Random RNG = new System.Random(1234);

        public static T RandomElement<T>(this IEnumerable<T> list, System.Random rng=null)
        {
            var arr = list.ToArray();
            var ix = (rng ?? RNG).Next(arr.Length);
            return arr[ix];
        }

        public static float Get(this float f)
        {
            return (float)(RNG.NextDouble() * f);
        }

        public static int RandomValue(this BoundedInteger number)
        {
            return RNG.Next(number.GetMin(), number.GetMax());
        }

        public static bool CoinFlip(this float f)
        {
            if (f >= 1)
                return true;
            else if (f <= 0)
                return false;

            var rnd = RNG.NextDouble();
            return rnd < f;
        }
    }

    /// <summary>
    /// Produces a random boolean by controlling the number of false results before the next true;
    /// </summary>
    public class RandomChance
    {
        private BoundedInteger MinMax;
        private int resultsBeforeNextTrue;

        public RandomChance(int min, int max)
        {
            MinMax = new BoundedInteger(min, max);
            resultsBeforeNextTrue = MinMax.RandomValue();
        }

        public bool GetNext()
        {
            if(resultsBeforeNextTrue <= 0)
            {
                resultsBeforeNextTrue = MinMax.RandomValue();
                return true;
            }

            resultsBeforeNextTrue--;
            return false;
        }


    }
}
