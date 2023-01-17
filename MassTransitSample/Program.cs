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

                    services.AddOptions<MassTransitHostOptions>().Configure(options =>
                    {
                        options.WaitUntilStarted = true;
                        options.StartTimeout = TimeSpan.FromSeconds(10);
                        options.StopTimeout = TimeSpan.FromSeconds(30);
                    });

                    services.AddHostedService<Worker>();
                });
    }
}