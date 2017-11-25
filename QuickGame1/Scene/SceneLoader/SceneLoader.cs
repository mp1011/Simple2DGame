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

        private Dictionary<string, QuickGameScene> loadedScenes = new Dictionary<string, QuickGameScene>();

        public QuickGameSceneLoader()
        {
        }

        Scene ISceneLoader.LoadScene(SceneID map, bool forceReload)
        {
            if (forceReload)
                loadedScenes.Remove(map.ToString());
           
            QuickGameScene ret = loadedScenes.TryGet(map.ToString());
            if (ret != null)
            {
                SetPlayerStart(ret);
                LastScene = ret;
                QuickGameScene.Current = ret;
                return ret;
            }
                
            if(MasterTemplate == null)
            {
                MasterTemplate = new MapTemplate("map", new Vector2(16,16));
            }

            if (map == Scenes.Intro)
                return new IntroScene();

            ret = new QuickGameScene(map, MasterTemplate);

            SetPlayerStart(ret);

            DebugText.NewTextPosition = new Vector2(50, 50);
            LastScene = ret;

            if(!forceReload)
                loadedScenes.Add(map.ToString(), ret);

            return ret;
        }

        private void SetPlayerStart(QuickGameScene newScene)
        {
            if (LastScene != null)
            {
                newScene.Player.Position.Center = CalculatePlayerStart(LastScene.Player, newScene);
                newScene.Player.Position.KeepWithin(newScene.Position, 16);
          //      newScene.Player.PutOnGround(newScene.TileMap,32);
                newScene.AdjustCamera();
                newScene.Player.Motion.Stop(Axis.X);
                newScene.Player.Motion.Stop(Axis.Y);

            }
        }

        private Vector2 CalculatePlayerStart(King playerInLastScene, Scene nextScene)
        {
            var templatePosition = MasterTemplate.PositionInMapToPointInTemplate(playerInLastScene.Scene.ID, playerInLastScene.Position.Center);

            var nextMapPosition = MasterTemplate.PointInTemplateToPositionInMap(nextScene.ID, templatePosition);

            return nextMapPosition;

        }
    }
}
