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
        UpdatePriority IUpdateable.Priority => UpdatePriority.MovingPlatformPositionCorrection;

        IRemoveable IUpdateable.Root => Actor;

        public MovingPlatformPositionAdjuster(T actor)
        {
            Actor = actor;
            actor.Layer.Scene.AddObject(this);            
        }
        
        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if (Actor.RidingBlock != null)
                Actor.Motion.CorrectPosition(Actor.RidingBlock.Motion.FrameVelocity);
        }
    }
}
