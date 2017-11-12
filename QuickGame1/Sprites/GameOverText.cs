using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class GameOverText : IMovingWorldObject
    {
        private GameText Text;

        public GameOverText(QuickGameScene scene) 
        {
            layer = scene.InterfaceLayer;
            motion = new MotionManager(this);

            Text = new GameText(Fonts.BigFont, "GAME OVER", scene.InterfaceLayer);
            Text.StayRelativeTo(this, 0, 0, this);
            
            Position = new GameEngine.Rectangle();

            var screen = Engine.GetScreenSize();
            new PathMover(this, screen.BottomCenter, screen.Center);           
        }

        private Layer layer;
        Layer IWorldObject.Layer => layer;

        private bool removed = false;
        bool IRemoveable.IsRemoved => removed;

        private MotionManager motion;
        MotionManager IMoveable.Motion => motion;

        Direction IWithPositionAndDirection.Direction { get; set; }

        public GameEngine.Rectangle Position { get; private set; }

        void IRemoveable.Remove()
        {
            this.removed = true;
        }
    }
}
