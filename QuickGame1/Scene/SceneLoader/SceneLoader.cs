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
                SetIntersceneActorPositions(ret);
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

            SetIntersceneActorPositions(ret);

            DebugText.NewTextPosition = new Vector2(50, 50);
            LastScene = ret;

            if(!forceReload)
                loadedScenes.Add(map.ToString(), ret);

            return ret;
        }

        private void SetIntersceneActorPositions(QuickGameScene newScene)
        {
            if (LastScene != null)
            {
                var isa = LastScene.InterSceneActors.Where(p => p.NextScene != null && p.NextScene.Equals(newScene.ID)).ToArray();
                foreach (var actorInLastScene in isa)
                {
                    var actorInNewScene = newScene.InterSceneActors.SingleOrDefault(p => p.GetType().Name == actorInLastScene.GetType().Name);
                    if(actorInNewScene == null)
                    {
                        actorInNewScene = (IMovesBetweenScenes)Activator.CreateInstance(actorInLastScene.GetType());
                    }

                    actorInNewScene.Position.Center = CalculateActorStart(actorInLastScene, LastScene.ID, newScene);
                    actorInNewScene.Position.KeepWithin(newScene.Position, 16);
                    actorInNewScene.Motion.Stop(Axis.X);
                    actorInNewScene.Motion.Stop(Axis.Y);
                    actorInNewScene.NextScene = null;
                }

                newScene.AdjustCamera();
            }
        }

        private Vector2 CalculateActorStart(IMovesBetweenScenes actorInLastScene, SceneID lastScene, Scene nextScene)
        {
            var templatePosition = MasterTemplate.PositionInMapToPointInTemplate(lastScene, actorInLastScene.Position.Center);

            var nextMapPosition = MasterTemplate.PointInTemplateToPositionInMap(nextScene.ID, templatePosition);

            return nextMapPosition;

        }
    }
}
