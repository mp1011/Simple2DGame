using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class QuickGameBoundaryTransition : BoundaryTransition
    {
        public override bool RequiresActor => true;

        private MapTemplate MasterTemplate;

        public QuickGameBoundaryTransition(QuickGameScene scene) : base(scene)
        {
            MasterTemplate = scene.MasterTemplate;
        }

        protected override SceneID GetNextMap(IMovesBetweenScenes actor, BorderSide exitSide)
        {
            var position = actor.Position.Center;

            switch(exitSide)
            {
                case BorderSide.Right:
                    position.X = (float)(Scene.Position.Right + actor.Position.Width*4);
                    break;
                case BorderSide.Left:
                    position.X = (float)(Scene.Position.Left - actor.Position.Width * 4);
                    break;
                case BorderSide.Top:
                    position.Y = (float)(Scene.Position.Top - actor.Position.Height * 4);
                    break;
                case BorderSide.Bottom:
                    position.Y = (float)(Scene.Position.Bottom + actor.Position.Height * 4);
                    break;
            }

            position = MasterTemplate.PositionInMapToPointInTemplate(CurrentMap, position);

            var nextMapIndex = MasterTemplate.MapRegions.FindIndex(p => p.Contains(position));
            if (nextMapIndex == -1)
                throw new ArgumentException("Unable to find adjacent map");

            return new SceneID(CurrentMap.Name, nextMapIndex);
        }
    }
}
