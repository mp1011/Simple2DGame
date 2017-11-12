using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Sign : MovingActor, IPlatformerObject, IShop
    {
        public ShopMenu ShopMenu { get; set; }
        public IMovingBlock RidingBlock { get; set; }
        public Sign() : base(QuickGameScene.Current, Textures.RockTiles)
        {
            Position.Set(0, 0, 16, 16);
            Position.Center = this.Position.Center;
            Position.Center.Translate(0, 16);

            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.None, 7,8,9);

            this.AddGravity();
        }
        private ManualCondition isUnderwater = new ManualCondition();
        public ManualCondition IsUnderWater => isUnderwater;

        public ManualCondition IsOnGround { get; set; } = new ManualCondition();

    }
}
