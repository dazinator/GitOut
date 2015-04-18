using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public abstract class IssueTrackerFactory
    {
        public abstract IssueTracker GetIssueTracker();
    }
}
