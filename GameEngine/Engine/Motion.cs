using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public class AbstractPosition : IWithPosition
    {
        public Rectangle Position { get; set; }
        public Layer Layer { get; set; }
        
        public AbstractPosition()
        {
            Position = new Rectangle();
        }
    }
    
    public enum Axis
    {
        Any=0,
        X,
        Y
    }


}
