# DependencyWalker for .NET

Show dependencies tree for .NET assemblies similar to how the old [Dependency Walker](http://www.dependencywalker.com/) shows them for non-managed applications.

To quickly start to use it, get the latest [DependencyWalker.zip](https://github.com/isindicic/DependencyWalker.Net/releases/download/1.0/DependencyWalker.zip) and unzip it on a local drive.
Simply execute DependWalker.exe with assembly filename as command line argument or just execute DependWalker.exe from explorer and then select assemblies using user interface. 

To successfully compile the solution you'll need Visual Studio 2010 and load DependencyWalker.Net.sln. 

Solution targets .NET 4.0 and application requires .Net Framework 4.0 installed in order to run. However, the application will work with assemblies that target lower version of framework without any problems.

**.NET5.0 support - BETA VERSION**

You are also able to compile program to target .NET5.0. This is still in development, but, in case you want to play with it, you'll need to run Visual Studio 2019 and load DependencyWalker.Net5.sln. 
