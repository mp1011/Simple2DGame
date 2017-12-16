using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IMovesBetweenScenes : IMoveable
    {
        SceneID NextScene { get; set; }
        void HandleTransition(Scene current, SceneID next);
    }
}
