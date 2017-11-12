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

    public struct CyclingInteger
    {
        public int Value;
        private int MinInc;
        private int MaxExc;

        public bool JustCycled { get; private set; }

        public CyclingInteger(int maxExc)
        {
            this.MinInc = 0;
            this.MaxExc = maxExc;
            this.Value = MinInc;
            this.JustCycled = false;
        }

        public CyclingInteger(int minInc, int maxExc)
        {
            MinInc = minInc;
            MaxExc = maxExc;

            Value = MinInc;
            JustCycled = false;
        }

        public override string ToString()
        {
            return $">={MinInc} : {Value} : <{MaxExc}";
        }

        public static CyclingInteger operator +(CyclingInteger c, int i)
        {
            if ((c.MaxExc - c.MinInc)==0)
                return c;

            int retVal = c.Value;
            retVal += i;
            c.JustCycled = false;

            while(retVal >= c.MaxExc)
            {
                c.JustCycled = true;
                retVal -= (c.MaxExc - c.MinInc);
            }

            c.Value = retVal;
            return c;
        }

        public CyclingInteger Reset()
        {
            Value = MinInc;
            JustCycled = false;
            return this;
        }

        public static CyclingInteger operator ++(CyclingInteger c)
        {
            if ((c.MaxExc - c.MinInc) == 0)
                return c;

            int retVal = c.Value;

            retVal++;
            c.JustCycled = false;
            if (retVal >= c.MaxExc)
            {
                c.JustCycled = true;
                retVal = c.MinInc;
            }

            c.Value = retVal;
            return c;
        }

        public static CyclingInteger operator --(CyclingInteger c)
        {
            if ((c.MaxExc - c.MinInc) == 0)
                return c;

            int retVal = c.Value;

            retVal--;
            c.JustCycled = false;
            if (retVal < c.MinInc)
            {
                c.JustCycled = true;
                retVal = c.MaxExc-1;
            }

            c.Value = retVal;
            return c;
        }

        public static implicit operator int(CyclingInteger c)
        {
            return c.Value;
        }

        public static bool operator ==(CyclingInteger c, int i)
        {
            return c.Value == i;
        }

        public static bool operator !=(CyclingInteger c, int i)
        {
            return c.Value != i;
        }
        

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CyclingInteger))
                return false;

            return this.Value == ((CyclingInteger)obj).Value;

        }

        public override int GetHashCode()
        {
            return this.Value;
        }
    }

    public struct CyclingDouble
    {
        public double Value;
        private double MinInc;
        private double MaxExc;

        public bool JustCycled { get; private set; }

        public CyclingDouble(double maxExc)
        {
            this.MinInc = 0;
            this.MaxExc = maxExc;
            this.Value = MinInc;
            this.JustCycled = false;
        }

        public CyclingDouble(double minInc, double maxExc)
        {
            MinInc = minInc;
            MaxExc = maxExc;

            Value = MinInc;
            JustCycled = false;
        }

        public override string ToString()
        {
            return $">={MinInc} : {Value} : <{MaxExc}";
        }

        public static CyclingDouble operator +(CyclingDouble c, double i)
        {
            if ((c.MaxExc - c.MinInc) == 0)
                return c;

            double retVal = c.Value;
            retVal += i;
            c.JustCycled = false;

            while (retVal >= c.MaxExc)
            {
                c.JustCycled = true;
                retVal -= (c.MaxExc - c.MinInc);
            }

            while (retVal < c.MinInc)
            {
                c.JustCycled = true;
                retVal += (c.MaxExc - c.MinInc);
            }

            c.Value = retVal;
            return c;
        }

        public CyclingDouble Reset()
        {
            Value = MinInc;
            JustCycled = false;
            return this;
        }

        public static CyclingDouble operator ++(CyclingDouble c)
        {
            if ((c.MaxExc - c.MinInc) == 0)
                return c;

            double retVal = c.Value;

            retVal++;
            c.JustCycled = false;
            if (retVal >= c.MaxExc)
            {
                c.JustCycled = true;
                retVal = c.MinInc;
            }

            c.Value = retVal;
            return c;
        }

        public static bool operator ==(CyclingDouble c, double i)
        {
            return c.Value == i;
        }

        public static bool operator !=(CyclingDouble c, double i)
        {
            return c.Value != i;
        }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CyclingDouble))
                return false;

            return this.Value == ((CyclingDouble)obj).Value;

        }

        public override int GetHashCode()
        {
            return (int)Value;
        }
    }

    public struct BoundedInteger
    {
        public int Value;
        private int Min;
        private int Max;

        public int GetMax()
        {
            return Max;
        }

        public int GetMin()
        {
            return Min;
        }

        public BoundedInteger SetToMax()
        {
            Value = Max;
            return this;
        }

        public BoundedInteger SetToMin()
        {
            Value = Min;
            return this;
        }

        public BoundedInteger(int max)
        {
            this.Min = 0;
            this.Max = max;
            this.Value = max;
        }

        public BoundedInteger(int minInc, int maxExc)
        {
            this.Min= minInc;
            this.Max = maxExc;
            this.Value = Min;
        }

        public override string ToString()
        {
            return $"{Value}/{Max}";
        }

        public static BoundedInteger operator +(BoundedInteger c, int i)
        {
            int retVal = c.Value;
            retVal += i;

            if (retVal > c.Max)
                retVal = c.Max;
            if (retVal < c.Min)
                retVal = c.Min;

            c.Value = retVal;
            return c;
        }

        public static BoundedInteger operator -(BoundedInteger c, int i)
        {
            int retVal = c.Value;
            retVal -= i;

            if (retVal > c.Max)
                retVal = c.Max;
            if (retVal < c.Min)
                retVal = c.Min;

            c.Value = retVal;
            return c;
        }

        public static BoundedInteger operator +(BoundedInteger c, double i)
        {
            return c + (int)i;
        }

        public static BoundedInteger operator -(BoundedInteger c, double i)
        {
            return c - (int)i;
        }

        public static BoundedInteger operator ++(BoundedInteger c)
        {
            int retVal = c.Value;

            retVal++;

            if (retVal > c.Max)
                retVal = c.Max;

            c.Value = retVal;
            return c;
        }

        public static bool operator ==(BoundedInteger c, int i)
        {
            return c.Value == i;
        }

        public static bool operator !=(BoundedInteger c, int i)
        {
            return c.Value != i;
        }

        public static implicit operator int(BoundedInteger c)
        {
            return c.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BoundedInteger))
                return false;

            return this.Value == ((BoundedInteger)obj).Value;

        }

        public override int GetHashCode()
        {
            return this.Value;
        }
    }

}
