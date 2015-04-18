using CommandLine;
using GitOut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.ReleaseNotesPlugin
{

    public class ReleaseNoteCreatorPlugin : CommandLinePlugin
    {

        public ReleaseNoteCreatorPlugin()
        {
            IssueTrackerCommandOptions = new Dictionary<string, IssueTrackerCommandOptions>();
            IssueTrackerCommandOptions.Add("github", new GitHubIssueTrackerCommandOptions());
            IssueTrackerCommandOptions.Add("jira", new JiraIssueTrackerCommandOptions());
            IssueTrackerCommandOptions.Add("youtrack", new YouTrackIssueTrackerCommandOptions());
            IssueTrackerCommandOptions.Add("bitbucket", new BitBucketIssueTrackerCommandOptions());
        }

        public override string CommandName
        {
            get { return "releasenotes"; }
        }

        private Dictionary<string, IssueTrackerCommandOptions> IssueTrackerCommandOptions = null;

        public override void Execute(IPluginExecutionContext context)
        {
            var args = context.Args;

            // Note that context can be shared between multiple plugins (if more than one is being executed)
            // so we have the option of storing output on it for another plugin to utilise.. 

            // we can parse these args using a different parser if we wanted, but we will stick with
            // the same one that GitOut.Core uses..           
            var parser = new CommandLine.Parser(with => with.IgnoreUnknownArguments = true);
            var options = new GitReleaseNotesCommandOptions();

            if (parser.ParseArguments(args, options))
            {
                // Now based on the issue tracker specified, parse the options for that specific tracker.
                if (IssueTrackerCommandOptions.ContainsKey(options.IssueTrackerName.ToLower()))
                {
                    var issueTrackerOptions = IssueTrackerCommandOptions[options.IssueTrackerName.ToLower()];
                    if (parser.ParseArguments(args, issueTrackerOptions))
                    {
                        // Now get the factory for the issue tracker in question.
                        var issueTrackerFactory = issueTrackerOptions.GetIssueTrackerFactory();

                        // Not get the issue tracker instance.
                        var issueTracker = issueTrackerFactory.GetIssueTracker();

                        // Do stuff with the issue tracker!
                        issueTracker.DoStuff();
                    }
                    else
                    {
                        // invalid args for this issue tracker.
                    }
                }
                else
                {
                    // unrecognised issue tracker specified.

                }

                return;
            }
            else
            {
                // context.LogError("");
                // Console.WriteLine("Failed to parse args for plugin: " + plugin.Title);
                return;
            }
        }

    }
}
