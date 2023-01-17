namespace Company.Consumers
{
    using MassTransit;

    public class MassTransitSampleConsumerDefinition :
        ConsumerDefinition<MassTransitSampleConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<MassTransitSampleConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}