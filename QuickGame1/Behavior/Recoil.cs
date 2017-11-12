using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    public static class Recoil
    {

        public static void RecoilFrom<TObject,TOther>(this TObject obj, TOther other)
            where TObject : IMoveable
            where TOther : IWithPositionAndDirection
        {
            obj.Direction = other.Direction.Opposite();

            var xMotion = obj.Motion.GetMotionByName<AxisMotion>("recoilX");
            if(xMotion == null)
            {
                xMotion = new AxisMotion("recoilX", obj).Set(deactivateWhenTargetReached: true, flipWhen:Direction.Left);
            }

            var yMotion = obj.Motion.GetMotionByName<AxisMotion>("recoilY");
            if (yMotion == null)
            {
                yMotion = new AxisMotion("recoilY", obj).Set(deactivateAfterStart: true);
            }

            xMotion.Active = false;
            xMotion.Active = true;

            yMotion.Active = true;                           
        }
    }
}
