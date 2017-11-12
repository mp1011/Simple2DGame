using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IMouseInput : IGameInput
    {
        Vector2 MousePosition { get; }
    }
}
