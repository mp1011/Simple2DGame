using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{
    class Interface : IUpdateable
    {
        private BorderedRectangle border;
        private IconCounter hearts;
        private QuickGameScene Scene;
        private GameText ScoreLabel;

        public Interface(QuickGameScene scene)
        {
            Scene = scene;

            var screen = Engine.GetScreenBoundary().Position;
                   
            var tileset = GameTiles.Border();
            border = new BorderedRectangle(tileset, new Rectangle(0, 0, screen.Width.SnapTo(tileset.Texture.CellSize.X), 30.SnapTo(tileset.Texture.CellSize.Y)), 
                scene.InterfaceLayer);

            hearts = new IconCounter(scene.Player.DamageHandler.Hitpoints.GetMax(), Textures.HeartTexture, 0, 1, 2, scene.InterfaceLayer);
            hearts.Position.SetCorner(8, 4);


            ScoreLabel = new DynamicText<King>(scene.Player, p => "SCORE: " + p.Score.ToString("000000"), Fonts.SmallFont, scene.InterfaceLayer);
           
            ScoreLabel.DockInside(border, BorderSide.Right).Nudge(-4, 0);

            scene.AddObject(this);
        }

        UpdatePriority IUpdateable.Priority => UpdatePriority.EndUpdate;

        IRemoveable IUpdateable.Root => Scene.InterfaceLayer;

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            hearts.Value = Scene.Player.DamageHandler.Hitpoints;
        }
    }
}
