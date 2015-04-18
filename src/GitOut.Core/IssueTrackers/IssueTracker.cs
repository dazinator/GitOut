using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitOut.Core
{
    public abstract class IssueTracker
    {
        public abstract string Name { get; }


        public void DoStuff()
        {
            Console.WriteLine("Doing lot's of {0} related issue tracker stuff right now!", Name);
        }
    }
}
