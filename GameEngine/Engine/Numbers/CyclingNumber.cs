using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
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
            if ((c.MaxExc - c.MinInc) == 0)
                return c;

            int retVal = c.Value;
            retVal += i;
            c.JustCycled = false;

            while (retVal >= c.MaxExc)
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
                retVal = c.MaxExc - 1;
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

}
