using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitOut.Core;
using CommandLine.Text;

namespace GitOut
{
    class Program
    {
        static void Main(string[] args)
        {
            // NOTES
            // 1. Start running in Debug mode in VS, I have set start up args (on project page) to be:

            // gitout.exe --plugin releasenotes --verbose --issuetracker "github" --accesstoken "sometoken" --reponame "myrepo"

            // you could also use:

            // gitout.exe --plugin semver 

            // I have also set a post-build event on the ReleaseNotesPlugin project and the SemanticVersionPlugin project,
            // so that their assemblies are copied into the GitOut.exe directory, so that they can be discovered at runtime as a plugins. 

            var parser = new CommandLine.Parser(with => with.IgnoreUnknownArguments = true);
            var appOptions = new Options();
            if (parser.ParseArguments(args, appOptions))
            {
                // parsing succeds
                // discover / load plugins
                var pluginManager = new PluginManager();
                var plugin = pluginManager.GetPluginInstanceByName(appOptions.PluginName);

                if (plugin == null)
                {
                    Console.WriteLine("Plugin not found for a command named: " + appOptions.PluginName);
                    return;
                }

                try
                {
                    // plugin execution context is context that we can pass to any plugin - and also could provide
                    // the ability for plugins to access shared / central state. 
                    // it could contain commonly needed things like IssueTrackers, Loggers, or a service locator etc.
                    var pluginExecutionContext = new PluginExecutionContext();
                    pluginExecutionContext.Args = args; // command line args defferred to the plugin.
                    // Execute the plugin.
                    plugin.Execute(pluginExecutionContext);
                }
                catch (Exception e)
                {
                    HandlePluginExecutionError(e);
                    return;
                }
            }
            else
            {
                // In addition to allowing particular plugins to be invoked on their own,
                // We could allow a workflow execution, based on a config file.
                // we could do that here by parsing a command that points to the yaml config
                // which would define which plugins were executed, with which settings, and in which order?
                Console.WriteLine("Failed to parse args, see usage.");
            }

        }

        private static void HandlePluginExecutionError(Exception e)
        {
            Console.WriteLine("Exception occurred during plugin execution - " + e.Message);
        }
    }
}
