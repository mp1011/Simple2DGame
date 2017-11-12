using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class Ext
    {
        public static void DestroyWhenOutOfBounds(this IWorldObject o)
        {
            new DestroyWhenOutOfBounds(o);
        }
    }

    class DestroyWhenOutOfBounds : IUpdateable
    {
        private IWorldObject Object;
        
        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => Object;

        public DestroyWhenOutOfBounds(IWorldObject o)
        {
            Object = o;
            o.Layer.Scene.AddObject(this);
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            var screen = Engine.GetScreenBoundary();
            if (screen.GetExitingSide(Object.Position, Object.Position.Width, Object.Position.Height) != BorderSide.None)
                Object.Remove();
        }
    }
}
