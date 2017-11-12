using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Fonts
    {
        private static SpriteFont _BigFont;
        public static SpriteFont BigFont
        {
            get
            {
                if(_BigFont == null)
                {
                    _BigFont = new SpriteFont(Textures.BigFontTexture);
                    _BigFont.SetMapFromString(" !*+,-./0123\"456789:;<=#>?@ABCDEFG$HIJKLMNOPQ%RSTUVWXYZ[&\\]^_'.(){|}~");
                }

                return _BigFont;
            }
        }

        private static SpriteFont _SmallFont;
        public static SpriteFont SmallFont
        {
            get
            {
                if (_SmallFont == null)
                {
                    _SmallFont = new SpriteFont(Textures.SmallFontTexture);
                    _SmallFont.SetMapFromString(" !*+,-./0123\"456789:;<=#>?@ABCDEFG$HIJKLMNOPQ%RSTUVWXYZ[&\\]^_'.(){|}~");
                }

                return _SmallFont;
            }
        }


    }
}
