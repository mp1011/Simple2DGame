using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Splash : Actor 
    {
        public Splash() : base(QuickGameScene.Current, Textures.SplashTexture)
        {
            Position.Set(0, 0, 32, 32);
            Position.Center = this.Position.Center;
            Position.Center.Translate(0, 16);

            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.None, 0,1,2,3);

            new DestroyOnAnimationFinished(this);
            
        }
    }
}
