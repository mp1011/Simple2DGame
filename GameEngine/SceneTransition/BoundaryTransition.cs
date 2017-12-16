using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class BoundaryTransition : SceneTransition     
    {
        protected Scene Scene { get; private set; }
        protected SceneID CurrentMap => Scene.ID;
        private Boundary ExitBounds;

        public override SceneID GetNextMap(IMovesBetweenScenes actor)
        {
            var exitSide = Scene.Boundary.GetExitedSide(actor.Position);
            if (exitSide != BorderSide.None)
                return GetNextMap(actor, exitSide);
            else
                return null;
        }

        protected abstract SceneID GetNextMap(IMovesBetweenScenes actor, BorderSide exitSide);

        public BoundaryTransition(Scene scene) 
        {
            Scene = scene;
        }
    }
}
