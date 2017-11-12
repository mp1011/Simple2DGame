using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class EnemyBullet : MovingActor, IDamager
    {
        public EnemyBullet() : base(QuickGameScene.Current, Textures.BulletTexture)
        {
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.None, 0, 1, 2);

            Position.SetWidth(8, AnchorOrigin.Left);
            Position.SetHeight(8, AnchorOrigin.Bottom);

            this.DestroyWhenOutOfBounds();
        }

        DamageType IDamager.DamageType => DamageType.EnemyAttack;
    }
}
