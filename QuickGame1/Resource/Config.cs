using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    static class QuickGameConfig
    {
        public static TimeSpan NormalAttackFrequency => Config.ReadValue<TimeSpan>("normal attack frequency");
    }
}
