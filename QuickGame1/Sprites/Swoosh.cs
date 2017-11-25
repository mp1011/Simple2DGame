using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Swoosh : MovingActor, IDamager, ITileBreaker
    {
        bool ITileBreaker.CanBreakTiles { get; set; } = true;

        DamageType IDamager.DamageType => DamageType.PlayerAttack;

        public Swoosh() : base(QuickGameScene.Current, Textures.SwooshTexture)
        { 
            Position.Set(0, 0, 32, 32);
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2, 3);
            new DestroyOnAnimationFinished(this);
        }
    }

  
}
