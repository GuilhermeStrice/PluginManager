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
    /// <typeparam name="R">Type of PluginResponseData that is going to get used to return a response from the plugin list</typeparam>
    /// <typeparam name="P">Type of PluginProcess data that is going to get used to process data</typeparam>
    public class PluginDllManager<T, M, R, P> : PluginManagerBase<T, M, R, P> where T : Plugin<R, P> where M : PluginDataManager 
                                           where R : PluginResponseData where P : PluginProcessData
    {
        /// <summary>
        /// Loads all plugins and their data from the plugin folder
        /// </summary>
        public override void Load()
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

                    // If it doesn't contain anything, this will not run
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
                    }
                }

                // It's faster if we store them instead of accessing the property
                pluginsSize = Plugins.Count;
            }
        }
    }
}
