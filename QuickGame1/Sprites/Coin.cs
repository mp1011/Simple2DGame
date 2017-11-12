using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace QuickGame1
{
    class Coin : Actor, IPrize, IEditorPlaceable
    {
        public Coin() : base(QuickGameScene.Current, Textures.CoinTexture)
        {
            Position.SetWidth(16, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(16, GameEngine.AnchorOrigin.Top);
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2, 3);            
        }

        SoundEffect IPrize.CollectSound => Sounds.GetCoin;

        CellType IEditorPlaceable.EditorType => CellType.Coin;
        
        void IPrize.OnCollected(ICanGetPrizes player)
        {
            player.Score += 25;
            player.Coins++;
        }
    }
}
