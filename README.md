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

The plugin, when executed, is entirely at liberty to parse the arguments it has been passed, how it sees fit.

Last thing to mention is that these plugins all reference GitOut.Core which can have common classes / issue trackers / shared services / logging etc. 






