using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IBlock : ICollidable { }

    public interface IMovingBlock : IBlock, IMoveable { }
}
