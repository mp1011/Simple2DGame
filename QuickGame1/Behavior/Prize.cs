using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    interface ICanGetPrizes : IMovingWorldObject
    {
        int Score { get; set; }
        int Coins { get; set; }
        DamageHandler DamageHandler { get; }
        bool HasBetterAttack { get; set; }
    }

    interface IPrize : ICollidable, IWorldObject<QuickGameScene>
    {
        SoundEffect CollectSound { get; }
        void OnCollected(ICanGetPrizes player);
    }     
}
