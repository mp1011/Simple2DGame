using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class XNAContentLoader<TKey, T> : IContentLoader<TKey, T> where TKey : IKey
    {
        protected ContentManager ContentManager { get; private set; }
        private string Folder;

        public XNAContentLoader(ContentManager manager, string folder)
        {
            ContentManager = manager;
            Folder = folder;
        }

        public T LoadContent(TKey key)
        {
            var ret = ContentManager.Load<T>($"{Folder}/{key.Name}");                
            return ret;
        }
    }
}
