using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Anything that can be drawn
    /// </summary>
    public interface IDisplayable :IWithPosition
    {
        void Draw(IRenderer painter);
        TextureDrawInfo DrawInfo { get; }
    }
    
    public interface IDynamicDisplayable : IDisplayable
    {
        IRemoveable Root { get; }
    }

}
