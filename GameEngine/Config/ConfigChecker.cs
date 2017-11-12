using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public interface IDependsOnConfig 
    {
        void ReloadFromConfig(IConfigurationProvider configProvider);
    }

    //not the best name
    public class ConfigRoot<T> : IRemoveable where T:IRemoveable
    {
        private int ChangeIndicator;
        private T Parent;
        private IConfigurationProvider ConfigProvider;

        public ConfigRoot(T parent, IConfigurationProvider configProvider)
        {
            ConfigProvider = configProvider;
            ChangeIndicator = configProvider.ChangeIndicator;
            Parent = parent;
        }

        bool IRemoveable.IsRemoved => Parent.IsRemoved || ChangeIndicator < ConfigProvider.ChangeIndicator;

        void IRemoveable.Remove()
        {
            Parent.Remove();
        }
    }

    public class ConfigChecker<T> : IUpdateable where T: IDependsOnConfig
    {
        public UpdatePriority Priority { get { return UpdatePriority.BeginUpdate; } }

        private T Parent;
        private IRemoveable Root;
        private IConfigurationProvider ConfigProvider;
        private int ChangeIndicator;

        IRemoveable IUpdateable.Root => Root;

        public ConfigChecker(IRemoveable root, T parent, IConfigurationProvider configProvider)
        {
            ConfigProvider = configProvider;
            Root = root;
            Parent = parent;
            LoadFromConfig();
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if (ConfigProvider.ChangeIndicator > ChangeIndicator)
                LoadFromConfig();
        }

        private void LoadFromConfig()
        {
            ChangeIndicator = ConfigProvider.ChangeIndicator;
            Parent.ReloadFromConfig(ConfigProvider);
        }
    }
}
