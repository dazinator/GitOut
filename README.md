# GitOut
(Demo for an Architecture idea)
Automate your Git Releases with this command line tool. Various plugins provide bridges between third party services.

# Pluggable

There are 2 sample plugins:

1. ReleaseNotes
2. SemanticVersion


These plugins are simple class libraries that derive from a base plugin class:

```
 public class ReleaseNoteCreatorPlugin : CommandLinePlugin
    {
    
       public override string CommandName
        {
            get { return "releasenotes"; }
        }
        
      public override void Execute(IPluginExecutionContext context)
        {
            var args = context.Args; // these are the arguments from the command line.

            // Note that IPluginExecutionContext could provide access to shared services, or shared state - if say
            // multiple plugins were executed as part of some workflow. 
        
             /// shortened for brevity
        }
   
    }
```

When GitOut.exe is invoked, it dynamically discovers plugin types by examining assemblies in the application directory. 
Therefore assemblies containing plugins, must be dropped in that location. 

GitOut.exe looks for a `--plugin` argument, and then looks for a plugin with the matching `CommandName` - it then Executes() that plugin, providing execution context for the plugin, which includes the full command arguments.

For example:

```
gitout.exe --plugin releasenotes --verbose --issuetracker "github" --accesstoken "sometoken" --reponame "myrepo"
```

This will result in the ReleaseNoteCreatorPlugin being executed.

Where as:

```
gitout.exe --semver
```

would result in the SemanticVersion plugin being executed - which looks like this:

```
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

```

The plugin, when executed, is entirely at liberty to parse the arguments it has been passed, how it sees fit. (although it would be easiest to use the same parser that GitOut uses, as the library is allread present etc)

Last thing to mention is that these plugins all reference GitOut.Core (as that's where the plugin base class is) which can have common classes / issue trackers / shared services / logging etc - but the way those services would be exposed to the plugin in question, would be via the IPluginExecutionContext)


# Running the solution
1. Just start it in visual studio, in Debug mode.
    1. Post-build events on the 2 plugin projects, will copy the plugin assemblies to the GitOut.exe directory (hopefully you have xcopy available on your dev environment :))
    2. Start up arguments have been specified under the project page for GitOut.exe, so that by default, running the solution will execute GitOut.exe with arguments necessary to execute the releasenotes plugin. This should allow you to step through withotu any hassle just to get an idea.



