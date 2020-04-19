using Crayon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PluginManager.Manager
{
    /// <summary>
    /// Class that handles all plugin handling
    /// </summary>
    /// <typeparam name="T">Type of the Plugin that is going to get loaded</typeparam>
    /// <typeparam name="M">Type of PluginDataManager that is going to get used to load data</typeparam>
    public class PluginManager<T, M> where T : Plugin where M : PluginDataManager
    {
        /// <summary>
        /// A list of plugins based on their name
        /// </summary>
        public List<T> Plugins { get; private set; }

        private int pluginsSize;

        private PluginDataManager dataManager;

        /// <summary>
        /// Specifies if you are using the plugin system or not
        /// </summary>
        public bool Enabled { get; set; }

        public PluginManager()
        {
            Plugins = new List<T>();
            dataManager = (PluginDataManager)Activator.CreateInstance(typeof(M), null);
            Enabled = true; // By default it's true
        }

        /// <summary>
        /// Loads all plugins and their data from the plugin folder
        /// </summary>
        public void Load()
        {
            if (Enabled)
            {
                if (string.IsNullOrEmpty(PluginManagerConfig.PluginFolderPath))
                    throw new ArgumentNullException("Plugins folder path", "You need to set a path for your plugins folder");

                // First run
                if (!Directory.Exists(PluginManagerConfig.PluginFolderPath))
                {
                    Directory.CreateDirectory(PluginManagerConfig.PluginFolderPath);
                    return; // Let's return here, no plugins available
                }

                // Ignores files in this folder
                var pluginDirectories = Directory.GetDirectories(PluginManagerConfig.PluginFolderPath);

                foreach (var pluginPath in pluginDirectories)
                {
                    // Should contain the plugin file
                    var files = Directory.GetFiles(pluginPath, "*.dll");
                    foreach (var dll in files)
                    {
                        try
                        {
                            // We try to load them if they have at least one class that inherits Plugin
                            var assembly = Assembly.LoadFrom(dll);

                            var types = assembly.GetTypes();

                            foreach (var type in types)
                            {
                                if (type.IsSubclassOf(typeof(T)))
                                    addPlugin(type);
                            }
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            Console.Error.WriteLine(Output.Red($"Failed to load {Path.GetFileName(dll)}. We will continue to load more assemblies"), ex);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(Output.Red($"Failed to load {Path.GetFileName(dll)}. We will continue to load more assemblies"), ex);
                        }
                    }
                }

                pluginsSize = Plugins.Count;
            }
            else
            {
                // Throw here?
            }
        }

        /// <summary>
        /// Saves plugin information to their data files
        /// </summary>
        public void Unload()
        {
            foreach (var plugin in Plugins)
            {
                try
                {
                    var dataFilePath = plugin.DataFilePath();

                    // If it's null it means it's not using a file
                    if (!string.IsNullOrEmpty(dataFilePath))
                        dataManager.SaveData(dataFilePath, plugin.Data);
                }
                catch (Exception)
                {
                    Console.Error.WriteLine(Output.Red($"Couldn't save data for {plugin.Name()}"));
                }
            }
        }

        /// <summary>
        /// Internal function that handles plugin instancing
        /// </summary>
        /// <param name="plugin"></param>
        internal void addPlugin(Type plugin)
        {
            Plugin pluginInstance = (T)Activator.CreateInstance(plugin);

            try
            {
                Plugins.Add((T)pluginInstance);

                var pluginDataFilePath = pluginInstance.DataFilePath();

                // If it's null we are not using plugin data
                if (!string.IsNullOrEmpty(PluginManagerConfig.PluginDataFolderPath) && !string.IsNullOrEmpty(pluginDataFilePath))
                {
                    var pluginData = dataManager.LoadData(Path.Combine(PluginManagerConfig.PluginDataFolderPath, pluginDataFilePath));

                    // This one shouldn't throw, loadData() will always exist
                    pluginInstance.loadData(pluginData);
                }

                Console.WriteLine($"Loaded {pluginInstance.Name()}");
            }
            catch (ArgumentException)
            {
                Console.Error.WriteLine(Output.Red($"{pluginInstance.Name()} is already loaded"));
            }
        }

        /// <summary>
        /// Executes DoProcessing on all plugins
        /// </summary>
        /// <param name="processData"></param>
        public void DoProcessing(PluginProcessData processData)
        {
            for (int i = 0; i < pluginsSize; i++)
            {
                Plugins[i].DoProcessing(processData);
            }
        }
    }
}
