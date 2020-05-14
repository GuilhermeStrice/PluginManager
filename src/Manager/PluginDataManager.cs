using System;
using System.Collections.Generic;
using System.Text;

namespace PluginManager
{
    /// <summary>
    /// This class implements all the plugin data information handling
    /// </summary>
    public abstract class PluginDataManager
    {
        public PluginDataManager()
        {
            // Need a constructor to use Activator.CreateInstance()
        }

        /// <summary>
        /// Loads the data for a specific plugin
        /// </summary>
        /// <param name="pluginName">The path to the plugin data file</param>
        /// <returns>Plugin data object if found, else return null, the plugin must handle this</returns>
        public abstract PluginData LoadData(string pluginDataFileName);

        /// <summary>
        /// Saves the data to the plugin data file
        /// </summary>
        /// <param name="pluginName">The path to the plugin data file</param>
        /// <param name="data">Data object to save</param>
        public abstract void SaveData(string pluginDataFileName, PluginData data);
    }
}
