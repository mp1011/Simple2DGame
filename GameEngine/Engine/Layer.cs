using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public sealed class Layer : IRemoveable
    {
        public Scene Scene { get; private set; }
        public IDisplayable[] FixedDisplayable = new IDisplayable[] { };        
        public Vector2 ScrollingCoefficient { get; private set;}

        private List<IDynamicDisplayable> DynamicDisplayable = new List<IDynamicDisplayable>();

        public List<ICollidable> CollidableObjects = new List<ICollidable>();

        bool IRemoveable.IsRemoved => Scene.IsFinished;

        public Layer(Scene scene)
        {
            Scene =scene;
        }

        public void AddObject(IDynamicDisplayable displayable)
        {
            DynamicDisplayable.Add(displayable);
            var c = displayable as ICollidable;
            if (c != null)
                CollidableObjects.Add(c);
        }

        public void Cleanup()
        {
            DynamicDisplayable.RemoveAll(p => p.Root.IsRemoved);
            CollidableObjects.RemoveAll(p => p.IsRemoved);
        }

        public Layer SetNormalScrolling()
        {
            ScrollingCoefficient = new Vector2(1.0f, 1.0f);
            return this;
        }

        public Layer SetParallaxScrolling(float x, float y)
        {
            ScrollingCoefficient = new Vector2(x,y);
            return this;
        }

        public Layer SetNoScrolling()
        {
            ScrollingCoefficient = new Vector2(0, 0);
            return this;
        }

        void IRemoveable.Remove()
        {
            throw new NotSupportedException();
        }

        public void Draw(IRenderer renderer)
        {
            foreach (var d in FixedDisplayable)
            {
                d.Draw(renderer);
            }

            foreach (var d in DynamicDisplayable)
            {
                if (!d.Root.IsRemoved)
                    d.Draw(renderer);
            }

        }
    }

}
