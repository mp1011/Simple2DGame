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

        private Condition IsAttacking;
        private Condition HasLanded;
        private Condition IsOnLadder;

        public AnimationController(TActor actor, Condition isAttacking, Condition hasLanded)
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
            if(IsAttacking)
            {
                Actor.Animations.CurrentKey = AnimationKeys.Attack;
                return;
            }

            if(IsOnLadder)
            {
                if (Actor.Motion.CurrentMotionPerSecond.Y == 0)
                    Actor.Animations.CurrentKey = AnimationKeys.ClimbStop;
                else
                    Actor.Animations.CurrentKey = AnimationKeys.Climb;

                return;
            }

            if(HasLanded)
            {
                Actor.Animations.CurrentKey = AnimationKeys.Land;
                return;
            }

            if(Actor.IsOnGround)
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
