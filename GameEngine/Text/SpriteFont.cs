using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public struct FontKey : IKey
    {
        public string Name { get; }

        public FontKey(string name)
        {
            this.Name = name;
        }
    }

    public class FontMap : GenericMap<FontKey,SpriteFont>
    {
    }
    
    public class SpriteFont
    {
        public TextureInfo Texture { get; private set; }   
        private Dictionary<char, int> CharMap = new Dictionary<char, int>();

        public SpriteFont(TextureInfo texture)
        {
            Texture = texture;
        }

        public void SetMapFromString(string text)
        {
            int index = 0;
            foreach(var character in text)
            {
                if (!CharMap.ContainsKey(character))
                    CharMap.Add(character, index);

                index++;
            }
        }

        public SpriteGrid GetStringSprite(string message)
        {
            var sg = new SpriteGrid
            {
                Texture = Texture,
                Cells = new ArrayGrid<int>(message.Length, message.Select(c =>
                {
                    int val;
                    if (CharMap.TryGetValue(c, out val))
                        return val;
                    else
                        return CharMap[' '];
                }))
            } ;


            return sg;

        }

    }

}
