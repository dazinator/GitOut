using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public class GithubIssueTracker : IssueTracker
    {
        public GithubIssueTracker(string username, string password, string accessToken = "")
        {

        }



        public override string Name
        {
            get
            {
                return "GitHub";
            }
           
        }
    }
}
