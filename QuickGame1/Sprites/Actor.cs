using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{

    class MovingActor : Actor, IMovingWorldObject<QuickGameScene>
    {
        public MotionManager Motion { get; private set; }
        
        public MovingActor(QuickGameScene scene, TextureInfo texture) : base(scene, texture)
        {
            Motion = new MotionManager(this);
        }

    }

    class Actor : IWorldObject<QuickGameScene>, ICollidable, IDynamicDisplayable, IWithSprite
    {
        public QuickGameScene Scene { get; private set; }

        public Sprite Sprite { get; private set; }

        public AnimationSet Animations { get; private set; }
        
        public UpdatePriority Priority { get { return UpdatePriority.Motion; } }

        public Rectangle Position { get; private set; }
        public Layer Layer { get; private set; }
        public Vector2 LastNonZeroMotion { get; private set; }
        public Direction Direction { get; set; }

        public IRemoveable Root => this;

         
        public Actor(QuickGameScene scene, TextureInfo texture) 
        {
            Scene = scene;
            Layer = scene.SolidLayer;
            Position = new Rectangle();
            Sprite = new Sprite(texture);
            scene.SolidLayer.AddObject(this);
            Animations = new AnimationSet(this);
            scene.AddObject(Animations);
        }

        TextureDrawInfo IDisplayable.DrawInfo => TextureDrawInfo.Fixed();
      
        void IDisplayable.Draw(IRenderer painter)
        {
            Animations.CurrentAnimation.Draw(painter);
        }

        private bool _isRemoved = false;
        bool IRemoveable.IsRemoved => _isRemoved; 

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            return Position.CollidesWith(collidingObject,ignoreEdges);
        }

        public void Remove()
        {
            _isRemoved = true;
        }
    }
  
}
