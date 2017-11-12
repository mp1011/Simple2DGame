using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Box : MovingActor, IPlatformerObject, IBlock
    {
        public Box() : base(QuickGameScene.Current, Textures.BoxTexture)
        {
            Position.SetWidth(12, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(12, GameEngine.AnchorOrigin.Top);
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0);

            this.AddGravity();
        }

        private ManualCondition isUnderwater = new ManualCondition();
        public ManualCondition IsUnderWater => isUnderwater;
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }

    }
}
