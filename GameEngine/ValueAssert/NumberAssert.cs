using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class NumberAssert
    {
        public static int MustBePositive(this int number)
        {
            if (number <= 0)
                throw new Exception("Number must be positive");
            return number;
        }
    }
}
