using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class LayoutExtensions
    {

        public static IWithPosition DockInside(this IWithPosition thisPos, IWithPosition otherPos, BorderSide side)
        {
            if (side == BorderSide.Top)
            {
                thisPos.Position.Center = otherPos.Position.Center;
                thisPos.Position.SetTop(otherPos.Position.Top);
                return thisPos;
            }
            else if(side == BorderSide.Right)
            {
                thisPos.Position.Center = otherPos.Position.Center;
                thisPos.Position.SetRight(otherPos.Position.Right);
                return thisPos;
            }
            else
                throw new NotSupportedException();
        }

        public static IWithPosition PutNextTo(this IWithPosition thisPos, IWithPosition otherPos, BorderSide side)
        {
            if (side == BorderSide.Bottom)
            {
                thisPos.Position.Center = otherPos.Position.Center;
                thisPos.Position.SetTop(otherPos.Position.Bottom);
                return thisPos;
            }
            else
                throw new NotSupportedException();
        }

        public static IWithPosition Nudge(this IWithPosition thisPos, float x, float y)
        {
            thisPos.Position.Translate(x, y);
            return thisPos;
        }
    }
}
