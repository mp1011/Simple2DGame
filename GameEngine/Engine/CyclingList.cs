using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class CyclingList<T> 
    {
        public List<T> Items { get; private set; }
        public CyclingInteger Index { get; private set; }
        
        public CyclingList()
        {
            Items = new List<T>();
        }

        public void AddItem(T item)
        {
            Items.Add(item);
            Index = new CyclingInteger(0, Items.Count);
        }

        public void AddRange(IEnumerable<T> items)
        {
            Items.AddRange(items);
            Index = new CyclingInteger(0, Items.Count);
        }
 
        public bool Any()
        {
            return Items.Any();
        }

        public void RemoveCurrent()
        {
            var cur = this.Current;
            Items.Remove(cur);

            var ix = this.Index.Value;
            Index = new CyclingInteger(0, Items.Count);
            Index = Index + ix;
        }

        public T Current
        {
            get
            {
                if (!Items.Any())
                    return default(T);
                return Items[Index.Value];
            }
        }

        public void Next()
        {
            Index++;
        }

        public void Reset()
        {
            Index = Index.Reset();
        }
    }
}
