using GameEngine;
using Microsoft.Xna.Framework;
using System;

namespace QuickGame1
{
    class Snake : MovingActor, IPlatformerObject, IDamageable, IDamager, IEditorPlaceable
    {
        public bool RecoilsWhenHit { get; set; }
        public ManualCondition IsUnderWater { get; set; } = new ManualCondition();
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }
        public DamageHandler DamageHandler { get; private set; }
        DamageType IDamageable.TakesDamageFrom => DamageType.PlayerAttack | DamageType.Trap;

        DamageType IDamager.DamageType => DamageType.Enemy;

        CellType IEditorPlaceable.EditorType => CellType.Snake;

        public Snake() : base(QuickGameScene.Current, Textures.SnakeTexture)
        {
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(16, GameEngine.AnchorOrigin.Top);

            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2, 3);

            new EnemyBehavior<Snake>(this, EnemyBehaviorFlags.MovesMore | EnemyBehaviorFlags.HasGravity);

            DamageHandler = new EnemyDamageHandler<Snake>(2, this);

        }
    }
}
