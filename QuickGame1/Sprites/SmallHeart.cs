﻿using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class SmallHeart : MovingActor, IPrize, IBounces
    {

        public ManualCondition GravityOn { get; } = new ManualCondition(true);

        public SmallHeart() : base(QuickGameScene.Current, Textures.SmallHeartTexture)
        {
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(8, GameEngine.AnchorOrigin.Top);
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0);

            this.AddGravity();
            this.PushInDirection(Direction.Up, Config.ReadValue<float>("prize move"));
        }

        SoundEffect IPrize.CollectSound => Sounds.PlayerHit;
       
        void IPrize.OnCollected(ICanGetPrizes player)
        {
            player.DamageHandler.Hitpoints++;
        }
    }
}
