using Crayon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PluginManager.Manager
{
    public class PluginClassManager<T, M, R, P> : PluginManagerBase<T, M, R, P> where T : Plugin<R, P> where M : PluginDataManager
                                           where R : PluginResponseData where P : PluginProcessData
    {
        /// <summary>
        /// Loads all plugins and their data from the plugin folder
        /// </summary>
        public override void Load()
        {
            if (Enabled)
            {
                // Is this slow?
                // I mean.... will this work
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsSubclassOf(typeof(T)))
                            addPlugin(type);
                    }
                }

                // It's faster if we store them instead of accessing the property
                pluginsSize = Plugins.Count;
            }
        }
    }
}
