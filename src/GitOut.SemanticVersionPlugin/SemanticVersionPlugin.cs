using GitOut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.SemanticVersionPlugin
{
    public class SemanticVersionPlugin : CommandLinePlugin
    {

        public SemanticVersionPlugin()
        {

        }

        public override string CommandName
        {
            get { return "semver"; }
        }

        public override void Execute(IPluginExecutionContext context)
        {
            var args = context.Args;

            // see release notes plugin for a more detailed implementation of a plugin.
        }

    }
}
