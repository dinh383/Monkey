var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// Build
///////////////////////////////////////////////////////////////////////////////
Task("Build")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings 
    {
        Configuration = configuration
    };

    var projects = GetFiles("./**/*.csproj");

    foreach(var project in projects)
	{
	    DotNetCoreBuild(project.GetDirectory().FullPath, settings);
    }
});

Task("Default")
	.IsDependentOn("Build");

RunTarget(target);