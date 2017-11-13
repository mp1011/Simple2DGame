using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class QuickGameSceneLoader : ISceneLoader
    {
        private MapTemplate MasterTemplate;
        private QuickGameScene LastScene;

        public QuickGameSceneLoader()
        {
        }

        Scene ISceneLoader.LoadScene(SceneID map)
        {
            if(MasterTemplate == null)
            {
                MasterTemplate = new MapTemplate("map", new Vector2(16,16));
            }

            if (map == Scenes.Intro)
                return new IntroScene();

            var ret = new QuickGameScene(map, MasterTemplate);

            if (LastScene != null)
            {
                ret.Player.Position.Center = CalculatePlayerStart(LastScene.Player, ret);
                ret.Player.Position.KeepWithin(ret.Position, 32);
                ret.Player.PutOnGround(ret.TileMap);
            }

            LastScene = ret;
            return ret;
        }

        private Vector2 CalculatePlayerStart(King playerInLastScene, Scene nextScene)
        {
            var templatePosition = MasterTemplate.PositionInMapToPointInTemplate(playerInLastScene.Scene.ID, playerInLastScene.Position.Center);

            var nextMapPosition = MasterTemplate.PointInTemplateToPositionInMap(nextScene.ID, templatePosition);

            return nextMapPosition;

        }
    }
}
