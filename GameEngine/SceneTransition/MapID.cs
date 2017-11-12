using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SceneID : IKey
    {
        public string Name { get; }
        public int MapNumber { get; }

        public SceneID(string name, int mapNumber=0)
        {
            Name = name;
            MapNumber = mapNumber;
        }

        public override string ToString()
        {
            if (MapNumber > 0)
                return $"{Name}-{MapNumber}";
            else
                return Name;
        }
    }
}
