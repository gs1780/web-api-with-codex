#!/usr/bin/env dotnet-script
#r "nuget: Microsoft.DotNet.Cli.Utils, 8.0.0"

using Microsoft.DotNet.Cli.Utils;

Command.Create("dotnet", "restore").Execute();
Command.Create("dotnet", "build").Execute();
Command.Create("dotnet", "run --no-build").Execute();
