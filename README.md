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

##Workflow idea

A workflow could be defined as the orchestration of multiple plugins being executed in some sort of sequence, with potentially state passing between them.

For example:

1. GitVersion Plugin (Calculates SemVer version numbers, and exposes them to CI build systems via stdout)
2. GitReleaseNotes Plugin (Creates Release Notes based on Issue Tracker of choice (it wants the version number)
3. GithubReleaseManager Plugin (Allows releases to be created and modified, and issues to be closed / milestones completed on GitHub - it wants the releasenotes text) 

In the above workflow, you'd invoke `GitOut.exe --yamlworkflow yamlworkflowname` once to run the workflow, which would be defined in a corresponding `yaml` file named `yamlworkflowname.yaml`:

```
# Workflow steps
"GitVersion": { url, "https://somerepourl.git", branch: "master" } # This line specifies plugin name to execute, and args.
"GitReleaseNotes:": { version: %GitVersion.SemVer%, issuetracker: jira, projectid, 123, username: user1, password: password1, verbose: true }
"GitReleaseManager:": { version: %GitVersion.SemVer%, releasenotes: %GitReleaseNotes.ReleaseNotesJson%, username: user1, password: password1, verbose: true }
```

GitOut.exe would run this workflow by locating the YAML file, and reading parsing each line which corresponds to a Plugin to be executed. It would load the corresponding plugin, and call its execute method (passingin the args) similar to what it allready does.

### Input / Output params

In the example yaml workflow file, notice the line for `GitReleaseNotes` specifies an input argument that refers to an output from a previous step: `%GitVersion.SemVer%`

One way (there may be better ways) I see this could work, is that when Plugins execute, such as the `GitVersion` plugin, they add to an `OutputParameters` dictionary, which would be a property of the `IPluginExecutionContext`.

Because `IPluginExecutionContext` is then passed to subsequent plugins executed during the workflow, it means that all subsequent plugins would have access to those `OutputParameters`.

Propose that plugins expose OutputParameters by adding those values to the dictionary, using the format: `pluginname.paramname` as the key for the value, as that matches the yaml syntax to refer to that param. 

When GitOut.exe executes each step / plugin in the workflow, it could substitue arguments that match the syntax `%pluginname.propertyname%` with the actual value from the `IPluginExecutionContext`'s `OutputParameters` OR.. the plugins themselves can check for output parameters in this dictionary, when parsing the arguments... You get the idea.. hopefully.

This means, outputs from previous plugins executing within a workflow, can be accessed by subsequent plugins, which may be handy for things like the `ReleaseNotes` text, and current `semantic version`information amongst other things. 
