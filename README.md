# NLog.Owin.Logging

[![Build status](https://ci.appveyor.com/api/projects/status/25xa6el222x7fhwe/branch/master?svg=true)](https://ci.appveyor.com/project/nlog/nlog-owin-logging/branch/master)
[![codecov.io](https://codecov.io/github/NLog/NLog.Owin.Logging/coverage.svg?branch=master)](https://codecov.io/github/NLog/NLog.Owin.Logging?branch=master)
[![Version](https://badge.fury.io/nu/NLog.Owin.Logging.svg)](https://www.nuget.org/packages/NLog.Owin.Logging)

NLog logging adapter for OWIN!

## Installation

There's a nuget package you can install this way:

> Install-Package NLog.Owin.Logging

## Using

To use the NLogAdapter with its default configuration:

```C#
using NLog.Owin.Logging;

public class Startup
{
	public void Configuration(IAppBuilder app)
	{
		app.UseNLog();
	}
}
```

The default translation table is:

| TraceEventType	| NLog Loglevel |
|-------------------|---------------|
| Critical			| Fatal			|
| Error				| Error 		|
| Warning			| Warn 			|
| Information		| Info 			|
| Verbose			| Trace 		|
| Start				| Debug 		|
| Stop				| Debug 		|
| Suspend			| Debug 		|
| Resume			| Debug 		|
| Transfer			| Debug 		|

If you'd like to customize this translation table you can supply a `Func<TraceEventType, LogLevel>` to the extension above.

```C#
using NLog.Owin.Logging;
using NLog;
using System.Diagnostics;

public class Startup
{
	public void Configuration(IAppBuilder app)
	{
		// make a warning out of every log message!
		app.UseNLog((eventType) => LogLevel.Warn);
	}
}
```

## Note / Information

(Added in version 1.1) 
Any `EventId` passed to this `Microsoft.Owin.Logging.ILogger.WriteCore()` implementation is passed down to NLog in the log event's properties, and can be written to output by adding the matching line in the Log appender layout:

```
${event-properties:item=EventId}
```

You can find more information about this topic in: https://github.com/NLog/NLog/wiki/EventProperties-Layout-Renderer

## Help / Contribution

If you found a bug, please create an issue. Want to contribute? Create a pull request!
