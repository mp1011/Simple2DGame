using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Grapeman : MovingActor, IPlatformerObject, IDamageable, IDamager, IEditorPlaceable
    {
        public bool RecoilsWhenHit { get; set; }
        public ManualCondition IsUnderWater { get; set; } = new ManualCondition();
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }
        public DamageHandler DamageHandler { get; private set; }
        DamageType IDamageable.TakesDamageFrom => DamageType.PlayerAttack | DamageType.Trap;
        public ManualCondition GravityOn { get; } = new ManualCondition(true);
        DamageType IDamager.DamageType => DamageType.Enemy;
        CellType IEditorPlaceable.EditorType => CellType.Grapeman;

        public Grapeman() : base(QuickGameScene.Current, Textures.GrapemanTexture)
        {
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(24, GameEngine.AnchorOrigin.Top);
            
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 0, 0, 7, 7, 7);
            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 1, 2, 3, 4);
            Animations.Add(AnimationKeys.Jump, this, TextureFlipBehavior.FlipWhenFacingLeft, 5, 6);
            Animations.Add(AnimationKeys.Fall, this, TextureFlipBehavior.FlipWhenFacingLeft, 8);
            Animations.Add(AnimationKeys.Land, this, TextureFlipBehavior.FlipWhenFacingLeft, 7);
            Animations.Add(AnimationKeys.Attack, this, TextureFlipBehavior.FlipWhenFacingLeft, 10,10,10,11,11,11,12,12,12);

            new EnemyBehavior<Grapeman>(this, EnemyBehaviorFlags.None);

            DamageHandler = new EnemyDamageHandler<Grapeman>(5, this);
            
        }
    }
}
