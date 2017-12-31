using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public class Ledge : Obstacle
    {
        protected override bool IsVertical => false;

        public Ledge(Tile groundTile) : base(groundTile, t => t.IsSolid && !t.GetAdjacent(BorderSide.Top).IsSolid) { }

        public bool ActorIsAboutToWalkOff(IMoveable movingObject)
        {
            var distance = movingObject.Position.Center.GetDistanceInDirection(Position.GetSidePoint(movingObject.Direction), movingObject.Direction);
            if (distance <= 0)
                return true;

            var distancePerFrame = movingObject.Motion.FrameVelocity.X.Abs();

            var framesUntilWalkOff = (int)distance / distancePerFrame;
            return framesUntilWalkOff <= 30;
        }

        public override Direction GetDirectionAwayFrom(IWithPositionAndDirection obj)
        {
            return obj.CardinalDirectionTowards(this, Axis.X);
        }
    }
}
