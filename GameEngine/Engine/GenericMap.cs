using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IKey
    {
        string Name { get; }
    }

    public interface IContentLoader<TKey, TNative>
    {
        TNative LoadContent(TKey key);
    }

    public abstract class GenericMap<TKey, TNative> where TKey : IKey
    {
        private Dictionary<string, TNative> map = new Dictionary<string, TNative>();

        protected GenericMap() { }

        public void SetMapping(TKey key, TNative deviceKey)
        {
            map[key.Name] = deviceKey;
        }

        public TNative GetMappedKey(TKey input)
        {
            TNative ret;
            if (map.TryGetValue(input.Name, out ret))
                return ret;
            else
            {
                ret = default(TNative);
                return ret;
            }
        }        
    }

}
