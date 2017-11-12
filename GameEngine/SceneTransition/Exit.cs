using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SceneTransition
    {
        public virtual SceneID NextMap { get; }
        public ICondition ExitCondition { get; }

        public SceneTransition(SceneID nextMap, ICondition exitCondition)
        {
            NextMap = nextMap;
            ExitCondition = exitCondition;
        }

        public SceneTransition(ICondition exitCondition)
        {
            ExitCondition = exitCondition;
        }
    }
}
