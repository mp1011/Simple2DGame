using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public enum AnchorOrigin
    {
        BottomCenter,
        Center,
        Left,
        Right,
        Bottom,
        Top
    }

    public class TextureDrawInfo
    {
        public bool Visible = true;
        public bool FlipX;
        public bool FlipY;

        public bool FlipOffsetsOnly = false;

        public float Opacity = 1.0f;

        private static TextureDrawInfo _fixed = new TextureDrawInfo();
        public static TextureDrawInfo Fixed()
        {
            return _fixed;
        }
    }
}
