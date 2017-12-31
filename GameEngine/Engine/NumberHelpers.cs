using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public static class NumberExtensions
    {

        public const float AlmostZero = 0.00001f;

        public static double Mod(this double number, double mod)
        {
            while (number < 0)
                number += mod;

            return number % mod;
        }

        public static double RotateAngleInDegreesTowards(this double angle, double target, double amount)
        {
            var dist1 = (angle - target).Mod(360.0);
            var dist2 = (target - angle).Mod(360.0);
            if (dist1 < dist2)
                return (angle - Math.Min(dist1, amount)).Mod(360.0); 
            else
                return (angle + Math.Min(dist2, amount)).Mod(360.0); 
        }

        public static int Unit(this int value)
        {
            if (value < 0)
                return -1;
            else if (value > 0)
                return 1;
            else
                return 0;
        }

        public static float Unit(this float value)
        {
            if (value.IsCloseTo(0))
                return 0;
            else if (value > 0)
                return 1;
            else
                return -1;
        }

        public static float Abs(this float number)
        {
            if (number < 0)
                return -number;
            else
                return number;
        }

        public static double SnapTo(this float number, float increment)
        {
            var d = (int)Math.Floor(number / increment);
            return d * increment;
        }

        public static double SnapTo(this double number, double increment)
        {
            var d = (int)Math.Floor(number / increment);
            return d * increment;
        }

        public static double SnapTo(this int number, double increment)
        {
            var d = (int)Math.Floor(number / increment);
            return d * increment;
        }

        public static bool IsCloseTo(this float number, float other)
        {
            var diff = Math.Abs(number - other);
            return diff <= 1;
        }

        public static bool IsCloseTo(this double number, double other)
        {
            var diff = Math.Abs(number - other);
            return diff <= 1;
        }

        public static float MatchSign(this float number, float other)
        {
            if (other > 0)
                return Math.Abs(number);
            else if (other < 0)
                return -Math.Abs(number);
            else
                return number;
        }

        /// <summary>
        /// Returns true if the absolute value of the given number is greater than the limit.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsOverLimit(this float number, float? limit)
        {
            if (number == 0 || !limit.HasValue && limit.Value < 0)
                return false;

            if (number > 0)
                return number > limit.Value;
            else
                return number < -limit.Value;
        }

        /// <summary>
        /// Adds or subtracts from the given number towards the target, but will never pass it.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="target"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static float Approach(this float number, float target, double amount)
        {
            return number.Approach(target, (float)amount);
        }        

        /// <summary>
        /// Adds or subtracts from the given number towards the target, but will never pass it.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="target"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static float Approach(this float number, float target, float amount)
        {
            float diff = Math.Abs(number - target);
            if (diff <= AlmostZero)
                return target;

            if (diff <= amount)
                return target;

            if (number > target)
                return number - amount;
            else
                return number + amount;
        }

        /// <summary>
        /// Adds or subtracts from the given number towards the target, but will never pass it. First aligns number to the nearest integer
        /// </summary>
        /// <param name="number"></param>
        /// <param name="target"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static float Approach(this float number, float target, int amount)
        {
            double decimalAmount = 0;
            if (number > target)
                decimalAmount = number - Math.Floor(number);
            else if (number < target)
                decimalAmount = Math.Ceiling(number) - number;

            if (decimalAmount == 0)
                decimalAmount = 1;

            return number.Approach(target, decimalAmount);
        }

       
        public static float KeepInsideRange(this float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;

            return value;
        }

        public static int ToIndex(this Point p, int columns)
        {
            return (columns * p.Y) + p.X;
        }
    }

   
   

}
