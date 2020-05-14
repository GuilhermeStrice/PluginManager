using Crayon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginManager.Manager
{
    public abstract class PluginManagerBase<T, M, R, P> where T : Plugin<R, P> where M : PluginDataManager
                                           where R : PluginResponseData where P : PluginProcessData
    {
        public PluginManagerBase()
        {
            DataManager = (M)Activator.CreateInstance(typeof(M), null);
            Plugins = new List<T>();
            Enabled = true; // By default it's true
        }

        /// <summary>
        /// Data manager object
        /// </summary>
        internal M DataManager { get; set; }

        /// <summary>
        /// A list of plugins based on their name
        /// </summary>
        public List<T> Plugins { get; internal set; }

        internal int pluginsSize;

        /// <summary>
        /// Specifies if you are using the plugin system or not
        /// </summary>
        public bool Enabled { get; set; }

        public abstract void Load();

        public virtual void Unload()
        {
            foreach (var plugin in Plugins)
            {
                try
                {
                    plugin.InvokeOnUnload();

                    if (plugin.UsingDataFile())
                    {
                        var dataFilePath = plugin.DataFileName();
                        DataManager.SaveData(dataFilePath, plugin.Data);
                    }
                }
                catch (Exception)
                {
                    Console.Error.WriteLine(Output.Red($"Couldn't save data for {plugin.Name()}"));
                }
            }
        }

        /// <summary>
        /// Executes DoProcessing on all plugins
        /// </summary>
        /// <param name="processData"></param>
        /// <returns>The response object</returns>
        public virtual R DoProcessing(P processData)
        {
            R response = (R)Activator.CreateInstance(typeof(R));
            for (int i = 0; i < pluginsSize; i++)
            {
                Plugins[i].DoProcessing(processData, ref response);
            }

            return response;
        }

        /// <summary>
        /// Internal function that handles plugin instancing
        /// </summary>
        /// <param name="plugin"></param>
        internal void addPlugin(Type type)
        {
            Plugin<R, P> plugin = (T)Activator.CreateInstance(type);

            var pluginName = plugin.Name();

            // check versions too?
            // and load the most recent one?
            for (int i = 0; i < pluginsSize; i++)
            {
                if (Plugins[i].Name() == pluginName)
                {
                    Console.Error.WriteLine(Output.Red($"{pluginName} is already loaded"));
                    return;
                }
            }

            Plugins.Add((T)plugin);

            if (plugin.UsingDataFile())
            {
                var pluginDataFilePath = plugin.DataFileName();
                if (string.IsNullOrEmpty(pluginDataFilePath))
                    throw new ArgumentNullException("Plugin Data File Path", "To use data files you must specify a name for the file");

                var pluginData = DataManager.LoadData(Path.Combine(PluginManagerConfig.PluginDataFolderPath, pluginDataFilePath));

                plugin.loadData(pluginData);
            }

            Console.WriteLine($"Loaded {pluginName}");
        }
    }
}
