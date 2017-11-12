using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IWorldObject : IWithPositionAndDirection, IRemoveable
    {
        Layer Layer { get; }
    }

    public interface IWorldObject<TScene> : IWorldObject 
        where TScene :Scene
    {
        TScene Scene { get; }
    }

    public interface IMovingWorldObject : IWorldObject, IMoveable
    {
    }

    public interface IMovingWorldObject<TScene> : IWorldObject<TScene>, IMovingWorldObject
        where TScene : Scene
    {
    }
}
