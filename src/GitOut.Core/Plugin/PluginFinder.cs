using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    /// <summary>
    /// Can find types in assemblies that implement the given contract. 
    /// </summary>
    /// <remarks>Serializable allows this class to be marshalled accross app domain boundaries, should
    /// you wish to load plugin assemblies within a seperate app domain then this is important.</remarks>
    [Serializable]
    public class PluginFinder
    {
        public IList<PluginInfo<TPluginType>> FindPlugins<TPluginType>(string pluginPath)
        {

            List<PluginInfo<TPluginType>> discoveredPluginTypes = new List<PluginInfo<TPluginType>>();

            foreach (string assemblyFilePath in Directory.GetFiles(pluginPath, "*.dll"))
            {

                var candidateAssembly = Assembly.LoadFile(assemblyFilePath);
                var pluginBaseType = typeof(TPluginType);

                foreach (Type t in candidateAssembly.GetTypes())
                {
                    if (pluginBaseType.IsAssignableFrom(t))
                    {
                        if (t.IsAbstract || t.IsInterface)
                        {
                            continue;
                        }
                        // plugin found.
                        var pluginInfo = new PluginInfo<TPluginType>();
                        pluginInfo.Type = t;
                        discoveredPluginTypes.Add(pluginInfo);
                        // could create instance..?
                    }
                }

            }

            return discoveredPluginTypes;
        }
    }
}
