using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public struct BoundedInteger
    {
        public int Value;
        private int Min;
        private int Max;

        public bool IsMax => Value == (Max - 1);

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
            Value = Max - 1;
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
            this.Value = max - 1;
        }

        public BoundedInteger(int minInc, int maxExc)
        {
            this.Min = minInc;
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

            if (retVal >= c.Max)
                retVal = c.Max - 1;
            if (retVal < c.Min)
                retVal = c.Min;

            c.Value = retVal;
            return c;
        }

        public static BoundedInteger operator -(BoundedInteger c, int i)
        {
            int retVal = c.Value;
            retVal -= i;

            if (retVal >= c.Max)
                retVal = c.Max - 1;
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

            if (retVal >= c.Max)
                retVal = c.Max - 1;

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
