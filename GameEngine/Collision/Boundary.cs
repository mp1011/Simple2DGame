using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Boundary : ICollidable
    {
        private IWithPosition Area;
      
        public Rectangle Position => Area.Position;

        public Boundary(IWithPosition area)
        {
            Area = area;
        }

        public bool IsRemoved => false;

        public bool DetectCollision(Rectangle movingObject, bool ignoreEdges)
        {
            return GetExitingSide(movingObject) != BorderSide.None;
        }

        public bool DetectFrameStartCollision(Rectangle movingObject)
        {
            return DetectCollision(movingObject,false);
        }

        public BorderSide GetExitingSide(Rectangle movingObject)
        {
            return GetExitingSide(movingObject, 0, 0);
        }

        public BorderSide GetExitingSide(Rectangle movingObject, double xThreshold, double yThreshold)
        {
            if (movingObject.Right > Area.Position.Right + xThreshold)
                return BorderSide.Right;

            if (movingObject.Bottom > Area.Position.Bottom + yThreshold)
                return BorderSide.Bottom;

            if (movingObject.Left < Area.Position.Left - xThreshold)
                return BorderSide.Left;

            if (movingObject.Top < Area.Position.Top - yThreshold)
                return BorderSide.Top;

            return BorderSide.None;

        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
