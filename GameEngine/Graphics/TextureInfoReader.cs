using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class TextureInfoReader
    {
        public static TextureInfoReader Instance { get; private set; }

        private XNAGameEngine engine;
        private XNAContentLoader<TextureID, Texture2D> textureLoader;

        internal TextureInfoReader(XNAGameEngine gameEngine, ContentManager contentManager)
        {
            engine = gameEngine;
            Instance = this;
            textureLoader = new XNAContentLoader<TextureID, Texture2D>(contentManager,"Textures");
        }

        public ArrayGrid<Color> TextureToColors(string id)
        {
            var texture = Load(id);
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            var ret = new ArrayGrid<Color>(texture.Width, data);
            return ret;
        }

        public Texture2D Load(string id)
        {       
            return textureLoader.LoadContent(new TextureID(id));
        }
    }
}
