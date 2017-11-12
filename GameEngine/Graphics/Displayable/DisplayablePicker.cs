using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{ 
    public abstract class DisplayableSet<T> : IDisplayable where T:IDisplayable
    {
        protected void Add(string key, T item)
        {
            Displayable.Add(key, item);
            if (Key == null)
                Key = key;
        }

        private IDisplayable Sprite;
        public TextureDrawInfo DrawInfo {  get { return Sprite.DrawInfo; } }

        public DisplayableSet(IDisplayable sprite)
        {
            Sprite = sprite;
        }

        private Dictionary<string, T> Displayable = new Dictionary<string, T>();
        
        protected bool ContainsKey(string key)
        {
            return Displayable.ContainsKey(key);
        }

        protected string Key { get; set; }

        protected T Current { get { return Displayable[Key]; } }

        Rectangle IWithPosition.Position { get { return Current.Position; } }
        
        void IDisplayable.Draw(IRenderer painter)
        {
            Current.Draw(painter);
        }
    }
}
