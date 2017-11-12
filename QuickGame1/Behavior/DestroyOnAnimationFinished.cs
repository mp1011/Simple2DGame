using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class DestroyOnAnimationFinished : IUpdateable
    {
        public UpdatePriority Priority { get { return UpdatePriority.Behavior; } }

        public Actor Actor { get; private set; }

        IRemoveable IUpdateable.Root => Actor;

        public DestroyOnAnimationFinished(Actor actor)
        {
            Actor = actor;
            actor.Scene.AddObject(this);
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if (Actor.Animations.CurrentAnimation.Finished)
                Actor.Remove();
        }
    }
}
