using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class DynamicText<T> : GameText, IUpdateable
        where T:IRemoveable
    {

        private T Object;
        private Func<T, string> GetText;

        UpdatePriority IUpdateable.Priority => UpdatePriority.BeginUpdate;

        IRemoveable IUpdateable.Root => Object;

        public DynamicText(T obj, Func<T,string> getText, SpriteFont font, Layer layer) : base(font, "", layer)
        {
            Object = obj;
            GetText = getText;
            layer.AddObject(this);
            layer.Scene.AddObject(this);

            Message = GetText(Object);
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            string text = GetText(Object);
            if (text != Message)
                Message = text;
        }
    }
}
