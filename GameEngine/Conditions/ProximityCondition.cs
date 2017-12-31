using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ProximityCondition : Condition
    {
        private IWorldObject ThisObject, OtherObject;
        private int MinDistance;

        public ProximityCondition(IWorldObject thisObject, IWorldObject otherObject, int minDistance)
        {
            ThisObject = thisObject;
            OtherObject = otherObject;
            MinDistance = minDistance;
        }

        public override bool IsActive => ThisObject.Position.Center.GetAbsoluteDistanceTo(OtherObject.Position.Center) <= MinDistance;
    }

    public static class PromityConditionExtensions
    {
        public static ICondition IsCloseTo(this IWorldObject thisObj, IWorldObject otherObj, int minDistance)
        {
            return new ProximityCondition(thisObj, otherObj, minDistance);
        }
    }
}
