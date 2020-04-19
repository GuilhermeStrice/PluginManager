using System;
using Xunit;
using PluginManager.Manager;
using TestPlugin;

namespace PluginManager.Test
{
    public class PDM : PluginDataManager
    {
        public override PluginData LoadData(string pluginDataFolder)
        {
            return null;
        }

        public override void SaveData(string yes, PluginData data)
        {
            
        }
    }

    class PPD : PluginProcessData
    {

    }

    public class PluginManagerTest
    {
        [Fact]
        public void Test1()
        {
            PluginManager<Class1, PDM> pluginManager = new PluginManager<Class1, PDM>();
            PluginManagerConfig.PluginFolderPath = "Plugins";
            pluginManager.Load();
            pluginManager.DoProcessing(new PPD());
            pluginManager.Unload();
        }
    }
}
