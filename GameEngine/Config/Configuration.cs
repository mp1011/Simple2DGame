using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
   

    public class ConfigValue<T>
    {
        public string Name { get; private set; }
           
        public bool Optional { get; private set; }

        public virtual T Value
        {
            get
            {
                return Config.Provider.GetValue<T>(this.Name, Optional);
            }
        }

        public ConfigValue(string name, bool optional=false)
        {
            this.Name = name;
            Optional = optional;
        }

        private int lastChangeIndicator = 0;
        /// <summary>
        /// Returns true if this value was changed. The next time this method is called it will return false, until the value is changed again
        /// </summary>
        /// <returns></returns>
        public bool CheckForChange()
        {
            if(Config.Provider.ChangeIndicator > lastChangeIndicator)
            {
                lastChangeIndicator = Config.Provider.ChangeIndicator;
                return true;
            }

            return false;
        }
    }

    public class IndexedConfigValue<T> : ConfigValue<T>
    {        
        public int Index { get; set; }

        public IndexedConfigValue(string name, int index) : base(name)
        {
            this.Index = index;
        }

        public override T Value => throw new NotImplementedException();
    }


    public class ConstValue<T> : ConfigValue<T>
    {
        private T value;

        public ConstValue(T v) : base("const")
        {
            value = v;
        }

        public override T Value => value;
    }

    public class ScaledFloat 
    {
        public float Scale;

        private float BaseValue;

        public ScaledFloat(float baseValue, float scaleValue)
        {
            BaseValue = baseValue;
            Scale = scaleValue;
        }
        
        public float Value
        {
            get
            {
                return BaseValue * Scale;
            }
        }
    }

    public class ConfigArray<T>
    {
        private string Key;
        private int LastChangeIndicator = 0;

        private T[] items;

        public T[] GetValues()
        {
            if (Config.Provider.ChangeIndicator > LastChangeIndicator)
            {
                LastChangeIndicator = Config.Provider.ChangeIndicator;

                var section = Config.Provider.GetSection(Key);           
                foreach (var index in Enumerable.Range(0, items.Length))
                {
                    T value = section.GetValue<T>(index.ToString(), true);
                    if (value != null)
                        items[index] = value;
                    else
                        items[index] = default(T);
                }
            }

            return items;
        }

        public ConfigArray(string key, int length)
        {
            Key = key;
            items = new T[length];
        }


    }


   
}
