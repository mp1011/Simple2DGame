using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    interface ICanClimb : IPlatformerObject
    {
        ManualCondition IsOnLadder { get; }
    }

    class LadderHandler
    {
        public static void CheckClimbStart(ICanClimb actor, IGameInputWithDPad input)
        {
            if(input.Pad.GetInputVector().Y != 0)
            {
                BeginClimb(actor);
            }
        }

        public static void CheckClimbStop(ICanClimb actor, IGameInputWithDPad input) 
        {
            if (input.GetButtonPressed(GameKeys.Jump))
            {
                EndClimb(actor);
            }
        }

        private static void BeginClimb(ICanClimb actor)
        {
            actor.IsOnLadder.Active = true;
            var gravity = actor.Motion.GetMotionByName<AxisMotion>("gravity").Require();
            gravity.Active = false;
            actor.Motion.Stop(Axis.Y);
        }

        public static void EndClimb(ICanClimb actor)
        {
            if (actor.IsOnLadder.Active)
            {
                actor.IsOnLadder.Active = false;
                var gravity = actor.Motion.GetMotionByName<AxisMotion>("gravity").Require();
                gravity.Active = true;
            }
        }
    }

    class LadderCollisionDetector : ICollidable
    {
        private IWithPosition[] ladders;

        bool IRemoveable.IsRemoved => false;

        public LadderCollisionDetector(QuickGameTileMap map)
        {
            map.Layer.CollidableObjects.Add(this);
            ladders = map.Tiles.Cells.Points.Select(pt => map.GetTileFromGridPoint(pt))
                .OfType<QuickGameTile>()
                .Where(p => p.IsLadder).ToArray();
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            foreach (var ladder in ladders)
            {
                if (collidingObject.CollidesWith(ladder.Position,ignoreEdges))
                    return true;
            }

            return false;
        }

        void IRemoveable.Remove()
        {
        }
    }
}
