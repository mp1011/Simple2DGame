using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IRemoveable
    {
        bool IsRemoved { get; }
        void Remove();
    }

    public class GameRoot : IRemoveable
    {
        private static GameRoot _current;
        public static GameRoot Current
        {
            get
            {
                return _current ?? (_current = new GameRoot());
            }
        }

        public Scene Scene => Engine.Instance.Scene;

        public bool IsRemoved => false;

        private GameRoot() { }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }

}
