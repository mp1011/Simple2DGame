using GameEngine;
using System;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{
    class Shrapnel : MovingActor, IWithGravity
    {
        public ManualCondition GravityOn { get; } = new ManualCondition(true);

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
            ul.Motion.AdjustImmediately(new DirectedMotion { AngleInDegrees = 45, DistancePerSecond = 60 });
            ur.Motion.AdjustImmediately(new DirectedMotion { AngleInDegrees = 45+90, DistancePerSecond = 60 });
            bl.Motion.AdjustImmediately(new DirectedMotion { AngleInDegrees = 45+180, DistancePerSecond = 60 });
            br.Motion.AdjustImmediately(new DirectedMotion { AngleInDegrees = 45+270, DistancePerSecond = 60 });
        }
    }
}
