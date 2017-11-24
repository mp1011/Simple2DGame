using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;

namespace QuickGame1
{
    class FrozenObject : IEditorPlaceable, ICollidable, IDynamicDisplayable
    {
        private IEditorPlaceable ActualObject;
        private QuickGameScene Scene;

        private FrozenObject(IEditorPlaceable actual, QuickGameScene scene)
        {
            ActualObject = actual;
            scene.SolidLayer.CollidableObjects.Add(this);
            scene.SolidLayer.AddObject(this);
            ActualObject.Remove();
            Scene = scene;
        }

        CellType IEditorPlaceable.EditorType => ActualObject.EditorType;

        Rectangle IWithPosition.Position =>  ActualObject.Position;

        bool isRemoved = false;
        bool IRemoveable.IsRemoved => isRemoved;

        IRemoveable IDynamicDisplayable.Root => this;

        TextureDrawInfo IDisplayable.DrawInfo => TextureDrawInfo.Fixed();

        void IRemoveable.Remove()
        {
            isRemoved = true;
        }

        public static FrozenObject Create(IEditorPlaceable actual, QuickGameScene scene)
        {
            return (actual as FrozenObject) ?? new FrozenObject(actual, scene);
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            return ActualObject.Position.CollidesWith(collidingObject, ignoreEdges);
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            var actualDisplayable = ActualObject as IDisplayable;
            if (actualDisplayable != null)
                actualDisplayable.Draw(painter);
        }

        public void Unfreeze()
        {
            isRemoved = true;
            var newObject = Activator.CreateInstance(ActualObject.GetType()) as IEditorPlaceable;
            newObject.Position.Center = ActualObject.Position.Center;
        }
    }
}
