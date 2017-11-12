using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class GroundMotion<T> : AxisMotion
        where T : IMoveable, IPlatformerObject
    {
        private IPlatformerObject PlatformerObject;

        public GroundMotion(string name, T objectToMove) : base(name, objectToMove)
        {
            PlatformerObject = objectToMove;
        }

        public override bool Active { get => PlatformerObject.IsOnGround.Active && base.Active; set => base.Active = value; }
    }
}
