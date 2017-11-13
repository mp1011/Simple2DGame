using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASoundEffect = Microsoft.Xna.Framework.Audio.SoundEffect;

namespace GameEngine
{
    public abstract class AudioEngine
    {
        public static AudioEngine Instance { get; private set; }

        public AudioEngine()
        {
            Instance = this;
        }

        public abstract void PlaySound(SoundEffect sound, float percentVolume=1.0f);

        public abstract void LoadSound(SoundID sound);
    }

    public class XNAAudioEngine : AudioEngine
    {
        public SoundBank<XNASoundEffect> SoundBank = new SoundBank<XNASoundEffect>();        
        public IContentLoader<SoundID, XNASoundEffect> soundLoader;

        private Dictionary<SoundID, List<SoundEffectInstance>> instances = new Dictionary<SoundID, List<SoundEffectInstance>>();

        public XNAAudioEngine(ContentManager contentManager)
        {
            soundLoader = new XNAContentLoader<SoundID, XNASoundEffect>(contentManager,"Sounds");
        }

        public override void PlaySound(SoundEffect sound, float percentVolume = 1.0f)
        {
            var instance = GetNextFreeInstance(sound.ID, sound.MaxSoundsAtOnce);
            if (instance != null)
            {
                instance.Volume = sound.Volume * percentVolume;
                instance.Play();
            }
        }

        public SoundEffectInstance GetNextFreeInstance(SoundID soundID, int maxInstances)
        {
            var list = instances[soundID];
            
            foreach(var instance in list)
            {
                if (instance.State == SoundState.Playing)
                    maxInstances--;

                if (maxInstances <= 0)
                    return null;

                if (instance.State == SoundState.Stopped)
                    return instance;
            }

            var newInstance = SoundBank.GetMappedKey(soundID).CreateInstance();
            list.Add(newInstance);
            return newInstance;

        }

        public override void LoadSound(SoundID sound)
        {
            if (instances.ContainsKey(sound))
                return;

            var soundContent = soundLoader.LoadContent(sound);
            SoundBank.SetMapping(sound,soundContent);
            instances.Add(sound, new List<SoundEffectInstance>());
        }
    }
}
