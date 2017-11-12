using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class DebugWatch : Condition
    {
        public ICondition baseCondition;

        public DebugWatch(ICondition condition)
        {
            baseCondition = condition;
        }

        public override bool IsActive
        {
            get
            {
                var ret = baseCondition.IsActive;

                if (baseCondition.WasJustActivated())
                    GlobalDebugHelper.NoOp();
                if(ret)
                    GlobalDebugHelper.NoOp();
                else
                    GlobalDebugHelper.NoOp();

                return ret;
            }
        }
    }

    public static class DebugWatchExt
    {
        public static ICondition DebugWatch(this ICondition condition)
        {
            return new DebugWatch(condition);
        }
    }
}
