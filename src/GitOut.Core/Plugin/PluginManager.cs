using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public class PluginManager
    {

        public string PluginAssemblyPath { get; set; }

        public PluginManager()
            : this(Assembly.GetExecutingAssembly().AssemblyDirectory())
        {
        }

        public PluginManager(string pluginAssemblyPath)
        {
            PluginAssemblyPath = pluginAssemblyPath;
            Plugins = new List<ICommandLinePlugin>();
            LoadPlugins();
        }

        public IList<ICommandLinePlugin> Plugins { get; set; }

        private void LoadPlugins()
        {
            var pluginFinder = new PluginFinder();
            var pluginTypes = pluginFinder.FindPlugins<ICommandLinePlugin>(PluginAssemblyPath);

            // Load plugin instances into list.
            foreach (var pluginTypeInfo in pluginTypes)
            {
                try
                {
                    var pluginInstance = pluginTypeInfo.CreatePluginInstance();
                    Plugins.Add(pluginInstance);
                }
                catch (Exception e)
                {
                    // Handle plugin load failure, but continue to try loading other plugins.
                    OnPluginLoadFailed(pluginTypeInfo, e);
                }
            }

            // Could create an instance of the plugin finder in a seperate AppDomain, so that
            // plugin types it discovers (assemblies it loads) can ultimately be unloaded after type info is discovered..
            // Not currently necessary so uncommenting for now.
            // AppDomain domain = AppDomain.CreateDomain("PluginLoader");  
            // PluginFinder finder = (PluginFinder)domain.CreateInstanceFromAndUnwrap(currentAssemblyDir, typeof(PluginFinder).FullName);           
            // AppDomain.Unload(domain);


        }

        protected virtual void OnPluginLoadFailed(PluginInfo<ICommandLinePlugin> pluginTypeInfo, Exception e)
        {
            //todo logging etc.
            Console.WriteLine("Failed to load plugin instance, {0}, plugin type: {1}", e.Message, pluginTypeInfo.Type.FullName);
        }

        public ICommandLinePlugin GetPluginInstanceByName(string name)
        {
            foreach (var p in Plugins)
            {
                var commandName = p.CommandName;
                if (!string.IsNullOrWhiteSpace(commandName))
                {
                    if (commandName.ToLowerInvariant() == name.ToLowerInvariant())
                    {
                        return p;
                    }
                }
            }

            return null;
        }

    }

}
