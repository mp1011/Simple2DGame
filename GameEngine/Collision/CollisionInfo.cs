using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class CollisionInfo
    {
        public Rectangle OriginalPosition { get; set; }
        public Vector2 OriginalVelocity { get; set; }
    }
}
