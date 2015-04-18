using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public abstract class CommandLinePlugin : ICommandLinePlugin
    {

        public CommandLinePlugin()
        {
            // CommandLineOptions = new CommandLineOptions>();
        }

        public virtual string Title
        {
            get
            {
                // Returns the title of the assembly for the derived plugin.
                string title = "";
                var assembly = this.GetType().Assembly;
                var titleAttribute = assembly
                   .GetCustomAttributes<AssemblyTitleAttribute>(inherit: false)
                   .FirstOrDefault();
                if (titleAttribute != null)
                {
                    title = titleAttribute.Title;
                }
                return title;
            }
        }

        public virtual string Description
        {
            get
            {
                // Returns the description of the assembly for the derived plugin.
                string description = "";
                var assembly = this.GetType().Assembly;
                var descriptionAttribute = assembly
                   .GetCustomAttributes<AssemblyDescriptionAttribute>(inherit: false)
                   .FirstOrDefault();
                if (descriptionAttribute != null)
                {
                    description = descriptionAttribute.Description;
                }
                return description;
            }
        }

        public virtual Version Version
        {
            get
            {
                // Returns the version of the assembly for the derived plugin.             
                var assembly = this.GetType().Assembly;
                return assembly.GetName().Version;
            }
        }

        public abstract void Execute(IPluginExecutionContext context);

        public abstract string CommandName { get; }
    }
}
