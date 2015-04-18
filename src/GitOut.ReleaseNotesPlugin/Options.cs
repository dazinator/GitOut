using CommandLine;
using GitOut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.ReleaseNotesPlugin
{
    public class GitReleaseNotesCommandOptions : CommandLineOptions
    {

        [Option("working-dir", HelpText = "The directory of the Git repository to determine the version for; " +
                     "if unspecified it will search parent directories recursively until finding a Git repository.")]
        public string WorkingDirectory { get; set; }

        [Option("verbose", HelpText = "Enables verbose logging.")]
        public bool Verbose { get; set; }

        [Option("alltags", HelpText = "Specifies that all tags should be included in the release notes, if not specified then only the issues since the last tag are included.")]
        public bool AllTags { get; set; }

        [Option("issuetracker", Required = true, HelpText = "Specifies the issue tracker used, possible Options: GitHub, Jira, YouTrack, BitBucket")]
        public string IssueTrackerName { get; set; }

        [Option("outputfile", HelpText = "The file to output release notes to.")]
        public string OutputFile { get; set; }

        [OptionList("categories", HelpText = "Allows additional labels to be treated as categories")]
        public string Categories { get; set; }

        [Option("version", HelpText = "Specifies the version to publish")]
        public string Version { get; set; }

    }

    public abstract class IssueTrackerCommandOptions
    {
        public abstract IssueTrackerFactory GetIssueTrackerFactory();
    }

    public class GitHubIssueTrackerCommandOptions : IssueTrackerCommandOptions
    {

        [Option("username", HelpText = "Issue tracker username")]
        public string Username { get; set; }

        [Option("password", HelpText = "Issue tracker password")]
        public string Password { get; set; }

        [Option("accesstoken", HelpText = "GitHub access token")]
        public string AccessToken { get; set; }

        [Option("reponame", HelpText = "GitHub Repository name, in Organisation/Repository format")]
        public string RepositoryName { get; set; }

        public override IssueTrackerFactory GetIssueTrackerFactory()
        {
            return new GitHubIssueTrackerFactory()
            {
                Username = this.Username,
                Password = this.Password,
                AccessToken = this.AccessToken
            };
        }
    }

    public class JiraIssueTrackerCommandOptions : IssueTrackerCommandOptions
    {
        [Option("filter", HelpText = "Jql query for closed issues you would like included if mentioned. Defaults to project = <YOURPROJECTID> AND (issuetype = Bug OR issuetype = Story OR issuetype = \"New Feature\") AND status in (Closed, Done, Resolved)")]
        public string JqlFilter { get; set; }

        [Option("serverurl", HelpText = "Url of Jira server")]
        public string ServerUrl { get; set; }

        [Option("projectid", HelpText = "Jira issue tracker project ID")]
        public string ProjectId { get; set; }

        public override IssueTrackerFactory GetIssueTrackerFactory()
        {
            // todo return a jira issue tracker factory.
            throw new NotImplementedException();
            //return new GitHubIssueTrackerFactory()
            //{
            //    Username = this.Username,
            //    Password = this.Password,
            //    AccessToken = this.AccessToken
            //};
        }

    }

    public class YouTrackIssueTrackerCommandOptions : IssueTrackerCommandOptions
    {
        [Option("serverurl", HelpText = "Url of YouTrack server")]
        public string ServerUrl { get; set; }

        [Option("filter", HelpText = "YouTrack filter for closed issues that you would like included if mentioned. Defaults to project:<YOURPROJECTID> State:Resolved State:-{{Won't fix}} State:-{{Can't Reproduce}} State:-Duplicate")]
        public string YouTrackFilter { get; set; }

        [Option("projectid", HelpText = "YouTrack issue tracker project ID")]
        public string ProjectId { get; set; }

        public override IssueTrackerFactory GetIssueTrackerFactory()
        {
            // todo return a jira issue tracker factory.
            throw new NotImplementedException();
            //return new GitHubIssueTrackerFactory()
            //{
            //    Username = this.Username,
            //    Password = this.Password,
            //    AccessToken = this.AccessToken
            //};
        }

    }

    public class BitBucketIssueTrackerCommandOptions : IssueTrackerCommandOptions
    {
        [Option("consumerkey", HelpText = "BitBuckets Consumer Key used for Oauth authentication")]
        public string ConsumerKey { get; set; }

        [Option("consumersecretkey", HelpText = "BitBuckets Consumer Secret Key used for Oauth authentication")]
        public string ConsumerSecretKey { get; set; }

        public override IssueTrackerFactory GetIssueTrackerFactory()
        {
            // todo return a jira issue tracker factory.
            throw new NotImplementedException();
            //return new GitHubIssueTrackerFactory()
            //{
            //    Username = this.Username,
            //    Password = this.Password,
            //    AccessToken = this.AccessToken
            //};
        }

    }

}
