using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Elf : MovingActor, IPlatformerObject, IShop, IEditorPlaceable, IMovesBetweenScenes
    {
        public ShopMenu ShopMenu { get; set; }
        private ManualCondition isUnderwater = new ManualCondition();
        public ManualCondition IsUnderWater => isUnderwater;
        public IMovingBlock RidingBlock { get; set; }
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public ManualCondition GravityOn { get; } = new ManualCondition(true);
        CellType IEditorPlaceable.EditorType => CellType.Elf;

        public Elf() : base(QuickGameScene.Current, Textures.ElfTexture)
        {
            this.AddGravity();
            
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(24, GameEngine.AnchorOrigin.Top);
                    
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0,0,0,7,7,7);
            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 1,2,3,4);
            Animations.Add(AnimationKeys.Jump, this, TextureFlipBehavior.FlipWhenFacingLeft, 5,6);
            Animations.Add(AnimationKeys.Fall, this, TextureFlipBehavior.FlipWhenFacingLeft, 8);
            Animations.Add(AnimationKeys.Land, this, TextureFlipBehavior.FlipWhenFacingLeft, 7);
            Animations.Add(AnimationKeys.Attack, this, TextureFlipBehavior.FlipWhenFacingLeft, 10,11,12);

            new AnimationController<Elf>(this, Condition.False, Condition.False);
            WaterHelper.AddWaterPhysics(this);

            //  new ElfController(this);
            new PlatformerPathFollowerBehavior(this, Scene.SinglePathPoints, new QuickGameNearbyTileChecker(this, Scene.TileMap), Scene.TileMap);
            Scene.InterSceneActors.Add(this);
        }

        SceneID IMovesBetweenScenes.NextScene { get; set; }

        void IMovesBetweenScenes.HandleTransition(Scene current, SceneID next)
        {
            Remove();
        }
    }

}
