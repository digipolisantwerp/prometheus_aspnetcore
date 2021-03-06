# Prometheus Toolbox

The toolbox provides an endpoint "/metrics" and maintains metric information for pulling Prometheus metrics 
of a .net core API-application.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration in Startup.Configure](#configuration-in-startupconfigure)
- [Provided metrics](#provided-metrics)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Prerequisites

* package targets .net standard 2.0 > usable starting from .NET Core 2.0 and .NET Framework 4.6.1-projects

## Installation

To add the toolbox to a project, you add the package to the csproj-file :

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Prometheus" Version="1.1.0" />
  </ItemGroup>
``` 

In Visual Studio you can also use the NuGet Package Manager to do this.

## Configuration in Startup.Configure

The use of the Prometheus app-metrics toolbox is added in the Configure method of the Startup class.
Call this method as the first statement in StartUp.Configure for accurate metrics. 
The order of the Use-statements determines the order of middleware execution.

The route for retrieving metrics is "/metrics" by default, but this can be changed during configuration:

``` csharp
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
	{
		app.UseMetrics("/status/metrics");		// leave parameter empty for the default metrics route
		
		// insert other middleware and code ...
	}
```

## Provided metrics

The listed metrics are the default metrics provided by Prometheus-Net NuGet Package plus a few custom metrics:
* counter http_request_count with labels "method - route - code". Code is the HTTP status code of the response.
* histogram http_request_duration_ms with labels "method - route - code" and divided in buckets.

The metrics can be pulled from the added "/metrics"-endpoint of your API.

## Contributing

Pull requests are always welcome, however keep the following things in mind:

- New features (both breaking and non-breaking) should always be discussed with the [repo's owner](#support). If possible, please open an issue first to discuss what you would like to change.
- Fork this repo and issue your fix or new feature via a pull request.
- Please make sure to update tests as appropriate. Also check possible linting errors and update the CHANGELOG if applicable.

## Support

Erik Seynaeve (<erik.seynaeve@digipolis.be>)
