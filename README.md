# Profiler.NET
A .NET Profiler that integrates in your code

[![Build](https://github.com/TheFrance99/Profiler.NET/actions/workflows/build.yml/badge.svg)](https://github.com/TheFrance99/Profiler.NET/actions/workflows/build.yml)

## Supported .Net versions: 
 - 6.0.x - .Net Core
 
 ## Requirements:
 - .Net Core 6.0, Microsoft.NET.App

## Work in Progress
### The project is very new and basic for now, i'm planning to add more features, both in the code and to the repository.

The plan is to have one day an advanced profiler that can be used even in a production environment, for now the project only contains an <code>Instrumentor</code>.

This tool allow to check how much time a function is taking.<br>
The tool records the informations into a <code>InstrumentationSession</code>.<br>
To read the data that the instrumentor is capturing you can emit the session trought an emitter and then plot it to your favourite time line visualizer.<br>
For now the only Emitter provided by the project is the <code>GoogleChromeInstrumentorJsonFormatSessionEmitter</code> that serialize the data recorded in the Google Chrome time line visualizer (chrome://tracing/) json format, the emitter writes the json to a <code>System.IO.Stream</code>, from there you can write it to a file and visualize it.

If this emitter is not the right one for your case you can expand the code by creating your own emitter that fulfill your need, you just need to create a new class that implements the <code>IInstrumentationSessionEmitter</code> interface and implement the <code>Emit(InstrumentationSession session)</code> method.

Here's an example of how to use the Instrumentor:
```csharp
using Profiler.NET.Instrumentation;
using Profiler.NET.Instrumentation.Emitters;
using System.IO;

........

using (var outStream = new MemoryStream())
{
    using (var instrumentor = Instrumentor.StartNew("test"))
    {
        var test = "Lorem Ipsum";
        instrumentor.ProfileFunc(() => test.ToUpper()); //profile a function that returns something
        instrumentor.ProfileFunc("with label", () => test.ToUpper()); //profile a function that returns something, with a label
        instrumentor.ProfileAction(() => test.ToUpper()); //profile a function that returns void
        instrumentor.ProfileAction("with label", () => test.ToUpper()); //profile a function that returns void, with a label
        instrumentor.Stop();

        using (var emitter = new GoogleChromeInstrumentorJsonFormatSessionEmitter(outStream))
        {
            emitter.Emit(instrumentor.Session);
        }
    }
    
    using (var fileStream = File.Create("Filepath.json"))
    {
        outStream.Seek(0, SeekOrigin.Begin);
        outStream.CopyTo(fileStream);
    }
}
```

## How to use the library

For now there isn't an automatic release system on this repo, to use the library you must clone the repo and compile it yourself.<br>
This is infact the first problem that i'm gonna fix by integrating an automatic release system to the repo and distribute the repo as a nuget package.

## Future plans
- Automatic release on nuget
- Memory profiler
- A system to handle all the profilers
- More emitters
- Support for other .Net versions (and .Net Framework)
- Remote profiling (in the future)

