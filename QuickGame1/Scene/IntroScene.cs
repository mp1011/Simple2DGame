using GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class IntroScene : Scene 
    {

        public IntroScene() : base(Scenes.Intro)
        {
            Load();
        }

        protected override IEnumerable<Layer> LoadLayers()
        {
            yield return new Layer(this);
        }

        protected override void LoadSceneContent()
        {
            var text = new GameText(Fonts.BigFont, "MIKE'S GAME", Layers[0]);
            text.Position.Center = Position.Center;
        }

        protected override IEnumerable<SceneTransition> LoadTransitions()
        {
            yield return new ConditionTransition(new AnyButtonPressedCondition(Input.GetInput(this)), Scenes.Map);                
        }
    }
}
