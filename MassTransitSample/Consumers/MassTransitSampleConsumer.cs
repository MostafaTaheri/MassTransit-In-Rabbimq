namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class MassTransitSampleConsumer :
        IConsumer<MassTransitSample>
    {
        readonly ILogger<MassTransitSampleConsumer> _logger;

        public MassTransitSampleConsumer(ILogger<MassTransitSampleConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MassTransitSample> context)
        {
            _logger.LogInformation("Recieved text: {Text}", context.Message.Value);
            return Task.CompletedTask;
        }
    }
}