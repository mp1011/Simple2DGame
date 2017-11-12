using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IPlatformerObject : IMovingWorldObject
    {
        IMovingBlock RidingBlock { get; set; }
        ManualCondition IsUnderWater { get; }
        ManualCondition IsOnGround { get; }
    }
}
