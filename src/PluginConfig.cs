using System;
using System.Collections.Generic;
using System.Text;

namespace PluginManager
{
    public static class PluginManagerConfig
    {
        /// <summary>
        /// Path to the folder where plugin data is going to be stored
        /// </summary>
        public static string PluginDataFolderPath { get; set; }

        /// <summary>
        /// Path to the folder where plugins are going to be stored
        /// </summary>
        public static string PluginFolderPath { get; set; }
    }
}
