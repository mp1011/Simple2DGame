using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public interface IWithSprite
    {
        Sprite Sprite { get; }
    }

    public class Sprite 
    {
        public TextureDrawInfo DrawInfo { get; private set; }
        public TextureInfo Texture { get; private set; }
        public int Cell { get; set; }
        
        public Sprite(TextureInfo texture)
        {
            Texture = texture;
            DrawInfo = new TextureDrawInfo();

        }
    }
}
