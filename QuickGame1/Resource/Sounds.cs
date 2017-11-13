using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    public static class Sounds
    {
        public static SoundEffect GetCoin { get; private set; }
        public static SoundEffect HitGround { get; private set; }
        public static SoundEffect Swish { get; private set; }
        public static SoundEffect EnemyDie { get; private set; }
        public static SoundEffect PlayerHit { get; private set; }
        public static SoundEffect Splash { get; private set; }

        public static SoundID[] GetAllSounds()
        {
            return new SoundID[] { GetCoin.ID, HitGround.ID, Swish.ID, EnemyDie.ID, PlayerHit.ID, Splash.ID };
        }

        static Sounds()
        {
            GetCoin = new SoundEffect("GetCoin");
            HitGround = new SoundEffect("HitGround",0.4f,1);
            Swish = new SoundEffect("swish");
            EnemyDie = new SoundEffect("enemydie");
            PlayerHit = new SoundEffect("playerhit",0.1f,1);
            Splash = new SoundEffect("splash");
        }
    }
}

