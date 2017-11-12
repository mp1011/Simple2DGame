using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IMotionAdjuster
    {
        string Name { get; }
        bool Active { get; set; }
        Vector2 AdjustMotionPerSecond(TimeSpan elapsedInFrame, Vector2 currentMotionPerSecond);
    }  
}
