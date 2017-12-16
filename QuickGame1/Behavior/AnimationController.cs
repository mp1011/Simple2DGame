using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class AnimationController<TActor> : IUpdateable
        where TActor : Actor, IPlatformerObject
    {
        private TActor Actor;

        private ICondition IsAttacking;
        private ICondition HasLanded;
        private ICondition IsOnLadder;

        public AnimationController(TActor actor, ICondition isAttacking, ICondition hasLanded)
        {
            Actor = actor;
            actor.Scene.AddObject(this);
            IsAttacking = isAttacking;
            HasLanded = hasLanded;

            var climber = actor as ICanClimb;
            if (climber != null)
                IsOnLadder = climber.IsOnLadder;
            else
                IsOnLadder = Condition.False;
        }

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => Actor;

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(IsAttacking.IsActiveAndNotNull())
            {
                Actor.Animations.CurrentKey = AnimationKeys.Attack;
                return;
            }

            if(IsOnLadder.IsActiveAndNotNull())
            {
                if (Actor.Motion.CurrentMotionPerSecond.Y == 0)
                    Actor.Animations.CurrentKey = AnimationKeys.ClimbStop;
                else
                    Actor.Animations.CurrentKey = AnimationKeys.Climb;

                return;
            }

            if(HasLanded.IsActiveAndNotNull())
            {
                Actor.Animations.CurrentKey = AnimationKeys.Land;
                return;
            }

            if(Actor.IsOnGround.Active)
            {
                if (Actor.Motion.CurrentMotionPerSecond.X == 0)
                    Actor.Animations.CurrentKey = AnimationKeys.Stand;
                else
                    Actor.Animations.CurrentKey = AnimationKeys.Walk;

            }
            else
            {
                if (Actor.Motion.CurrentMotionPerSecond.Y < 0)
                    Actor.Animations.CurrentKey = AnimationKeys.Jump;
                else
                    Actor.Animations.CurrentKey = AnimationKeys.Fall;
            }
        }
    }
}
