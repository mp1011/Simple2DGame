using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class SceneTransition
    {
        public abstract bool RequiresActor { get; }
        public abstract SceneID GetNextMap(IMovesBetweenScenes actor);
    }
}
