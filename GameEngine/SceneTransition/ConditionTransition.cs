using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ConditionTransition : SceneTransition
    {
        public override bool RequiresActor => false;

        private ICondition Condition;
        private SceneID Scene;

        public ConditionTransition(ICondition condition, SceneID scene)
        {
            Condition = condition;
            Scene = scene;
        }

        public override SceneID GetNextMap(IMovesBetweenScenes actor)
        {
            if (Condition.IsActiveAndNotNull())
                return Scene;
            else
                return null;
        }
    }
}
