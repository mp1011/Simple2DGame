using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Slime : MovingActor, IPlatformerObject, IDamageable, IDamager, IEditorPlaceable
    {
        public bool RecoilsWhenHit { get; set; }
        public ManualCondition IsUnderWater { get; set; } = new ManualCondition();
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }
        public DamageHandler DamageHandler { get; private set; }
        DamageType IDamageable.TakesDamageFrom => DamageType.PlayerAttack | DamageType.Trap;

        DamageType IDamager.DamageType => DamageType.Enemy;

        CellType IEditorPlaceable.EditorType => CellType.Slime;

        public Slime() : base(QuickGameScene.Current, Textures.SlimeTexture)
        {
            Position.SetWidth(40, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(24, GameEngine.AnchorOrigin.Top);

            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingRight, 0,0,2,2,4,4,6,6);
           
            new EnemyBehavior<Slime>(this, EnemyBehaviorFlags.HasGravity | EnemyBehaviorFlags.Moves | EnemyBehaviorFlags.Shoots);

            DamageHandler = new EnemyDamageHandler<Slime>(5, this);

            //new DebugRectangle(this);
        }
    }
}
