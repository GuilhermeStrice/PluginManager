using System;
using System.Collections.Generic;
using System.Text;

namespace PluginManager
{
    public delegate void OnLoadedEvent(PluginData data);

    /// <summary>
    /// Base class to create plugins
    /// </summary>
    public abstract class Plugin
    {
        /// <summary>
        /// Event executed when the plugin manager finishes loading this plugin
        /// </summary>
        public event OnLoadedEvent OnLoaded;

        /// <summary>
        /// The plugin data object
        /// </summary>
        public PluginData Data { get; private set; }

        public Plugin()
        {
            // Need a constructor to use Activator.CreateInstance()
        }

        /// <summary>
        /// Internal function that handles data loading
        /// </summary>
        /// <param name="data">Data object that is going to get loaded</param>
        internal void loadData(PluginData data)
        {
            if (data == null)
                BuildDataFile();
            this.Data = data;
            OnLoaded?.Invoke(data);
        }

        /// <summary>
        /// Builds the default plugin file for when the plugin is first instanciated
        /// </summary>
        public abstract void BuildDataFile();

        /// <summary>
        /// The version of the plugin
        /// </summary>
        /// <returns>The version of the plugin</returns>
        public abstract Version Version();

        /// <summary>
        /// The version of the plugin
        /// </summary>
        /// <returns>The name of the plugin</returns>
        public abstract string Name();

        /// <summary>
        /// The path to the plugin data file
        /// </summary>
        /// <returns>The path to the plugin data file</returns>
        public abstract string DataFilePath();

        /// <summary>
        /// Base function that should get called everytime this plugin needs to process data
        /// </summary>
        /// <param name="data"></param>
        public abstract void DoProcessing(PluginProcessData data);
    }
}
