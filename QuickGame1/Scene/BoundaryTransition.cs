using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class QuickGameBoundaryTransition : BoundaryTransition<King>
    {
        private MapTemplate MasterTemplate;

        public QuickGameBoundaryTransition(King player, QuickGameScene scene) : base(player, scene)
        {
            MasterTemplate = scene.MasterTemplate;
        }

        protected override SceneID GetNextMap(BorderSide exitSide, King player)
        {
            var playerPosition = player.Position.Center;

            switch(exitSide)
            {
                case BorderSide.Right:
                    playerPosition.X = (float)(Scene.Position.Right + player.Position.Width*4);
                    break;
                case BorderSide.Left:
                    playerPosition.X = (float)(Scene.Position.Left - player.Position.Width * 4);
                    break;
                case BorderSide.Top:
                    playerPosition.Y = (float)(Scene.Position.Top - player.Position.Height * 4);
                    break;
                case BorderSide.Bottom:
                    playerPosition.Y = (float)(Scene.Position.Bottom + player.Position.Height * 4);
                    break;
            }

            playerPosition = MasterTemplate.PositionInMapToPointInTemplate(CurrentMap, playerPosition);

            var nextMapIndex = MasterTemplate.MapRegions.FindIndex(p => p.Contains(playerPosition));
            if (nextMapIndex == -1)
                throw new ArgumentException("Unable to find adjacent map");

            return new SceneID(CurrentMap.Name, nextMapIndex);
        }
    }
}
