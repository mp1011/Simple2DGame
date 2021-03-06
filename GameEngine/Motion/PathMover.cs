﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Moves an object along a path
    /// </summary>
    public class PathMover : IUpdateable
    {
        private float DistancePerSecond = 50;

        private Vector2[] Path;
        private IMovingWorldObject MovingObject;
        private BoundedInteger TargetNode;
       
        public PathMover(IMovingWorldObject movingObject, params Vector2[] path)
        {
            MovingObject = movingObject;
            Path = path;
            TargetNode = new BoundedInteger(path.Length-1).SetToMin();       
            movingObject.Layer.Scene.AddObject(this);
            MovingObject.Position.Center = path.First();
        }

        UpdatePriority IUpdateable.Priority => UpdatePriority.Motion;

        IRemoveable IUpdateable.Root => MovingObject;

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            var targetPoint = Path[TargetNode.Value];

          
            if(MovingObject.Position.Center.GetAbsoluteDistanceTo(targetPoint) <= DistancePerSecond * elapsedInFrame.TotalSeconds)
            {
                MovingObject.Position.Center = targetPoint;
                TargetNode++;
                //throw new NotImplementedException();
               // Motion.DistancePerSecond = 0;
            }
            else
            {
                MovingObject.MoveInDirection(MovingObject.Position.Center.GetDegreesTo(targetPoint), DistancePerSecond);
            }
            
        }
    }

    
}
