using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class StayRelativeTo : IUpdateable
    {
        private IWithPosition Parent;
        private IWithPosition Object;
        private Vector2 Offset;

        UpdatePriority GameEngine.IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => Root;
        private IRemoveable Root;

        public StayRelativeTo(IWithPosition objectToMove, IWithPosition origin, Vector2 offset, IWorldObject root)
        {
            Parent = origin;
            Offset = offset;
            Object = objectToMove;
            root.Layer.Scene.AddObject(this);
            Root = root;
        }

        void GameEngine.IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Object.Position.Center = Parent.Position.Center;
        }
    }

    public class StayRelativeTo<TParent> : IUpdateable where TParent : IWithPositionAndDirection
    {
        private TParent Parent;
        private IWorldObject Object;
        private Vector2 Offset;

        UpdatePriority GameEngine.IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => Object;

        public StayRelativeTo(IWorldObject objectToMove, TParent origin, Vector2 offset)
        {
            Parent = origin;
            Offset = offset;
            Object = objectToMove;
            objectToMove.Layer.Scene.AddObject(this);
        }

        void GameEngine.IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Object.Position.Center = Parent.Position.Center.Translate(Offset.FlipIfLeft(Parent.Direction));
            Object.Direction = Parent.Direction;
        }
    }

    public static class PRT
    {
        public static void StayRelativeTo<TParent>(this IWorldObject item, TParent parent, float xOff, float yOff)
            where TParent : IWithPositionAndDirection
        {
            new StayRelativeTo<TParent>(item, parent, new Vector2(xOff, yOff));
        }

        public static void StayRelativeTo(this IWithPosition item, IWithPosition parent, float xOff, float yOff, IWorldObject root)
        {
            new StayRelativeTo(item, parent, new Vector2(xOff, yOff), root);
        }
    }
}
