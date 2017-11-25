using GameEngine;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{
    class Shrapnel : MovingActor
    {
        private Shrapnel(TextureInfo texture) : base(QuickGameScene.Current, texture)
        {            
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingRight, 0);

            this.AddGravity();
            this.DestroyWhenOutOfBounds();
        }


        public static void CreateBlockShrapnel(Vector2 location)
        {
            CreateFourWayShrapnel(Textures.BrokenBlockTexture, location);
        }

        private static void CreateFourWayShrapnel(TextureInfo texture, Vector2 location)
        {
            var ul = new Shrapnel(texture);
            var ur = new Shrapnel(texture);
            var bl = new Shrapnel(texture);
            var br = new Shrapnel(texture);

            ul.Position.Center = location;
            ur.Position.Center = location;
            bl.Position.Center = location;
            br.Position.Center = location;

            ul.Direction = Direction.Left;
            bl.Direction = Direction.Left;
            ur.Direction = Direction.Right;
            br.Direction = Direction.Right;

            int speed = 60;
            ul.MoveInDirection(new Vector2(-1f, -1f), speed);
            ur.MoveInDirection(new Vector2(1f, -1f), speed);
            bl.MoveInDirection(new Vector2(-1f, 0f), speed);
            br.MoveInDirection(new Vector2(1f, 0f), speed);
        }
    }
}
