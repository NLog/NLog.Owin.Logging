# NLog.Owin.Logging

The missing NLog logging adapter for OWIN!

## Installation

There's a nuget package you can install this way:

> this will be back soon

Currently there are dependencies on:

| Target framework | Dependencies |
|---|---|
| .NET 4.5 | NLog (>= 4.0.0.0), Owin (>= 1.0) |

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
		app.UseNLog((lvl) => LogLevel.Warn);
	}
}
```

## Help / Contribution

If you found a bug, please create an issue. Want to contribute? Create a pull request!