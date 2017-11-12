using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    public static class AnimationKeys
    {
        public static AnimationKey Stand = new AnimationKey("stand");
        public static AnimationKey Walk = new AnimationKey("walk");
        public static AnimationKey JumpStart = new AnimationKey("jumpstart");
        public static AnimationKey Jump = new AnimationKey("jump");
        public static AnimationKey Fall = new AnimationKey("fall");
        public static AnimationKey Land = new AnimationKey("land");
        public static AnimationKey Attack = new AnimationKey("attack");
        public static AnimationKey Climb = new AnimationKey("climb");
        public static AnimationKey ClimbStop = new AnimationKey("climbstop");

    }
}
