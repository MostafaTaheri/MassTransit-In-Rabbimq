# MassTransit 

> A functioning installation of the dotnet runtime and sdk (at least 6.0).

## <br/>RabbitMQ:
A high performance transport that allows both cloud based and local development.

## <br/>Add Packages

Add the MassTransit and MassTransit.RabbitMQ packages to the project.

``` 
$ dotnet add package MassTransit

$ dotnet add package MassTransit.RabbitMQ
```

## <br/>Install MassTransit Templates

MassTransit includes project and item [templates](https://masstransit-project.com/usage/templates.html) simplifying the creation of new projects. Install the templates by executing dotnet new -i MassTransit.Templates at the console.

```
$ dotnet new --install MassTransit.Templates
```

# <br/>Initial Project Creation
## Create the worker project

To create a service using MassTransit, create a worker via the Command Prompt.

```
$ dotnet new mtworker -n MassTransitSample
$ cd MassTransitMassSample
$ dotnet new mtconsumer
```

## Overview of the code

When you open the project you will see that you have 3 class files.

* `Program.cs` is the standard entry point and here we configure the host builder.
* `Consumers/GettingStartedConsumer.cs` is the MassTransit [Consumer](https://masstransit-project.com/usage/consumers.html)
* `Contracts/MassTransitSample.cs` is an example messag

## Add A BackgroundService

In the root of the project add Worker.cs

```c#
using MassTransit;

namespace MassTransitSample
{

    public class Worker : BackgroundService
    {
        readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _bus.Publish(new Contracts.MassTransitSample { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
```

## Configure Services

In `Program.cs` at `CreateHostBuilder` method.

```c# 
using MassTransit;
using MassTransitSample;
using Company.Consumers;

namespace MassTransitSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MassTransitSampleConsumer>();
                        x.UsingRabbitMq((context, cfg) =>
                        {

                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("admin");
                                h.Password("admin");
                            });
                            cfg.ConfigureEndpoints(context);

                        });
                    });

                     services.AddHostedService<Worker>();
                });
    }
}
```

`localhost` is where the docker image is running. We are inferring the default port of `5672` and are using `\` as the [virtual](https://www.rabbitmq.com/vhosts.html) host (opens new window). `guest` and `guest` are the default username and password to talk to the cluster and management [dashboard](http://localhost:15672/#/) (opens new window).

# </br>Run the Project

```
$ dotnet run
```


