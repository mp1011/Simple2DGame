using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class MovingPlatformPositionAdjuster<T> : IUpdateable
        where T : IPlatformerObject, IWorldObject
    {
        private T Actor;
        UpdatePriority IUpdateable.Priority => UpdatePriority.Motion;

        IRemoveable IUpdateable.Root => Actor;

        public MovingPlatformPositionAdjuster(T actor)
        {
            Actor = actor;
            actor.Layer.Scene.AddObject(this);

            this.DebugWatch(Fonts.SmallFont, (actor.Layer.Scene as QuickGameScene).InterfaceLayer, x => x.debugText);
        }

        private string debugText = "";
        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(Actor.RidingBlock != null)
            {               
                Actor.Position.Translate(Actor.RidingBlock.FrameMotion());

                if (Actor.Motion.MotionPerSecond.Y >= 0 && Actor.Position.Bottom.IsCloseTo(Actor.RidingBlock.Position.Top))
                {
                    Actor.Position.SetBottom(Actor.RidingBlock.Position.Top);
                    Actor.Motion.Stop(Axis.Y);
                }    
            }
        }
    }
}
