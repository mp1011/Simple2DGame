using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public struct InputKey : IKey
    {
        public string Name { get; }

        public InputKey(string name)
        {
            this.Name = name;
        }
    }

    public class InputMap<TDeviceKeyType> : GenericMap<InputKey,TDeviceKeyType> 
    {
    }
    
    public interface IGameInput
    {
        bool GetButtonDown(InputKey key);
        bool GetButtonPressed(InputKey key);
        object GetPressedButton();
    }
    
    public abstract class InputDevice<TDeviceKeyType> : IGameInput, IUpdateable
    {
        public UpdatePriority Priority { get { return UpdatePriority.Input; } }

        private InputMap<TDeviceKeyType> KeyMap;

        public IRemoveable Root => GameRoot.Current;

        protected InputDevice(InputMap<TDeviceKeyType> keyMap, Scene scene)
        {
            KeyMap = keyMap;
            scene.AddObject(this);
        }
        
        object IGameInput.GetPressedButton()
        {
            return GetPressedButton();
        }

        bool IGameInput.GetButtonDown(InputKey key)
        {
            return GetButtonDown(KeyMap.GetMappedKey(key));
        }

        bool IGameInput.GetButtonPressed(InputKey key)
        {
            return GetButtonPressed(KeyMap.GetMappedKey(key));
        }

        protected abstract object GetPressedButton();
        protected abstract bool GetButtonDown(TDeviceKeyType key);
        protected abstract bool GetButtonPressed(TDeviceKeyType key);

        protected abstract void Update(TimeSpan frameTime);

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Update(elapsedInFrame);
        }
    }

}