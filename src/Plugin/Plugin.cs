using System;
using System.Collections.Generic;
using System.Text;

namespace PluginManager
{
    public delegate void OnLoadEvent();
    public delegate void OnLoadedEvent(PluginData data);
    public delegate void OnUnloadEvent();

    /// <summary>
    /// Base class to create plugins
    /// </summary>
    public abstract class Plugin<R, P> where R : PluginResponseData where P : PluginProcessData
    {
        /// <summary>
        /// Event executed when plugin object is instanciated
        /// </summary>
        public event OnLoadEvent OnLoad;

        /// <summary>
        /// Event executed when the plugin manager finishes loading this plugin
        /// </summary>
        public event OnLoadedEvent OnLoaded;

        /// <summary>
        /// Event executed when DataManager starts unloading the plugin
        /// </summary>
        public event OnUnloadEvent OnUnload;

        /// <summary>
        /// The plugin data object
        /// </summary>
        public PluginData Data { get; private set; }

        public Plugin()
        {
            OnLoad?.Invoke();
        }

        internal void InvokeOnUnload()
        {
            OnUnload?.Invoke();
        }

        /// <summary>
        /// Internal function that handles data loading
        /// </summary>
        /// <param name="data">Data object that is going to get loaded</param>
        internal void loadData(PluginData data)
        {
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
        /// Specifies whether we are using a data file or not
        /// </summary>
        /// <returns>Whether we are using a data file or not</returns>
        public abstract bool UsingDataFile();

        /// <summary>
        /// The path to the plugin data file
        /// </summary>
        /// <returns>The path to the plugin data file</returns>
        public abstract string DataFileName();

        /// <summary>
        /// Base function that should get called everytime this plugin needs to process data
        /// </summary>
        /// <param name="data">The data the plugin needs to do processing</param>
        /// <param name="response">Response object that the plugin will append to</param>
        /// <returns>The same object that is passed to the response parameter</returns>
        public abstract ref R DoProcessing(P data, ref R response);
    }
}
