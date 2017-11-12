using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameEngine
{
    public struct SoundID : IKey
    {
        public string Name { get; private set; }

        public SoundID(string name)
        {
            Name = name;
        }
    }

    public class SoundBank<TSound> : GenericMap<SoundID, TSound>
    {
    }
    
    public class SoundEffect
    {
        public SoundID ID { get; private set; }
        public float Volume { get; private set; }
        public int MaxSoundsAtOnce { get; private set; }

        public SoundEffect(string name) : this(name,1.0f,3)
        {
        }

        public SoundEffect(string name, float volume, int maxSoundsAtOnce)
        {
            ID = new SoundID(name);
            Volume = volume;
            MaxSoundsAtOnce = maxSoundsAtOnce;
        }
    }
}
