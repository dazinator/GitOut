using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public interface ICommandLinePlugin
    {
        string Title { get; }
        string Description { get; }
        Version Version { get; }

        string CommandName { get; }

        void Execute(IPluginExecutionContext pluginOptions);
    }

    public interface IPluginExecutionContext
    {
        string[] Args { get; set; }

    }

    public class PluginExecutionContext : IPluginExecutionContext
    {
        public string[] Args { get; set; }
    }

}
