using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut
{
    class Options
    {
        [Option('p', "plugin", Required = true, HelpText = "Plug-In to activate.")]
        public string PluginName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var help = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            // help.AddPreOptionsLine("<<license details here.>>");
            help.AddPreOptionsLine("Usage: GitOut -p SomePlugin");
            help.AddOptions(this);
            return help;
        }
    }
}
