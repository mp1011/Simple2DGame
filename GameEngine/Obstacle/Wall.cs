using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Wall : Obstacle
    {
        protected override bool IsVertical => true;

        public Wall(Tile groundTile, Direction actorDir) : base(FindWallTile(groundTile, actorDir), 
            t => t.IsSolid && !t.GetAdjacent(actorDir).IsSolid) { }

        private static Tile FindWallTile(Tile groundTile, Direction dir)
        {
            var tile = groundTile.GetAdjacent(BorderSide.Top).GetFirstAdjacent(dir, p => p.IsSolid);
            return tile;
        }

        public bool ActorCouldJumpOver(IMoveable movingObject)
        {
            //figure out a better way to calc this
            return Position.Height < 64;
        }

        public bool ActorIsAboutToWalkInto(IMoveable movingObject)
        {
            var distance = movingObject.Position.Center.GetDistanceInDirection(Position.GetSidePoint(movingObject.Direction.Opposite()), movingObject.Direction);
            if (distance <= movingObject.Position.Width)
                return true;

            var distancePerFrame = movingObject.Motion.FrameVelocity.X.Abs();

            var framesUntilHit = (int)distance / distancePerFrame;
            return framesUntilHit <= 30;
        }

        public override Direction GetDirectionAwayFrom(IWithPositionAndDirection obj)
        {
            return obj.DirectionAwayFrom(this, Axis.X);
        }
    }
}
