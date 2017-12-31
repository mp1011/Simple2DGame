using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Hitbox : IWithPosition
    {
        private IWithPositionAndDirection Owner;

        public Rectangle Position { get; }

        public ICondition Condition { get; } 

        public Hitbox(ICondition condition, IWorldObject owner)
        {
            Condition = condition;
            Owner = owner;
            Position = new Rectangle(0, 0, 16, 32); //todo

            this.StayRelativeTo(owner, 16, 0, owner); //todo
        }
    }

    /// <summary>
    /// Hitbox that is activated when a certain animation is playing
    /// </summary>
    public class AnimationControlledHitbox : Hitbox
    {
        public AnimationControlledHitbox(IWorldObject owner, AnimationSet animations, AnimationKey key, int frameMin, int frameMax) 
            : base(new PlayingAnimationCondition(animations, key, frameMin, frameMax), owner) { }
    }
}
