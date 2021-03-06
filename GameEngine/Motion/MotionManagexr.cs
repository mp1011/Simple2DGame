﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public class MotionManager : IUpdateable
    {
        public Vector2 MotionPerSecond { get; private set; }
        public Vector2 LastNonZeroMotion { get; private set; }
        public Vector2 FrameVelocity { get; private set; }
        public Rectangle FrameStartPosition { get; private set; }  
        public IMovingWorldObject ObjectToMove { get; private set; }

        UpdatePriority IUpdateable.Priority => UpdatePriority.Motion;
        IRemoveable IUpdateable.Root => ObjectToMove;
       
        public MotionManager(IMovingWorldObject objectToMove)
        {
            ObjectToMove = objectToMove;
            objectToMove.Layer.Scene.AddObject(this);
            FrameStartPosition = new Rectangle();
        }
        
        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            FrameStartPosition.Set(ObjectToMove.Position);

            foreach (var force in Forces)
            {
                if(force.Active)
                    MotionPerSecond = force.AdjustMotionPerSecond(elapsedInFrame, MotionPerSecond);
            }

            FrameVelocity = CalcFrameVelocity(elapsedInFrame);

            ObjectToMove.Position.Center = ObjectToMove.Position.Center.Translate(FrameVelocity);

            if (MotionPerSecond.X != 0 || MotionPerSecond.Y != 0)
                LastNonZeroMotion = MotionPerSecond;            
        }

        private Vector2 CalcFrameVelocity(TimeSpan elapsedInFrame)
        {
            var frameMotion = MotionPerSecond.Scale(elapsedInFrame.TotalSeconds);
            return frameMotion;
        }

        public void Stop(Axis a)
        {
            MotionPerSecond = MotionPerSecond.SetAxis(a, 0);
        }

      
    }
}
