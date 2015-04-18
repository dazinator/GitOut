using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public class GitHubIssueTrackerFactory : IssueTrackerFactory
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }

        public override IssueTracker GetIssueTracker()
        {          
            return new GithubIssueTracker(Username, Password, AccessToken);
        }
    }

   
}
