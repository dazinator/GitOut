using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{

    public class PluginInfo<T>
    {
        /// <summary>
        /// The plugin's type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Creates an instance of the plugin and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreatePluginInstance()
        {
            T plugin = (T)Activator.CreateInstance(Type);
            return plugin;
        }


    }

}
