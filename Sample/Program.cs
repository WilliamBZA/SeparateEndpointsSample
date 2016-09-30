using System;
using System.Threading.Tasks;
using NServiceBus;
using Sample;
using ServiceControl.Plugin.CustomChecks;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {

        var highPriorityEndpointConfig = new EndpointConfiguration("Samples.ComplexSagaFindingLogic.HighPriority");
        highPriorityEndpointConfig.UseSerialization<JsonSerializer>();
        highPriorityEndpointConfig.EnableInstallers();
        highPriorityEndpointConfig.UsePersistence<InMemoryPersistence>();
        highPriorityEndpointConfig.SendFailedMessagesTo("error2@machinename");
        highPriorityEndpointConfig.AuditProcessedMessagesTo("audit2");

        Console.Title = "Samples.ComplexSagaFindingLogic";
        var endpointConfiguration = new EndpointConfiguration("Samples.ComplexSagaFindingLogic");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error2");
        endpointConfiguration.AuditProcessedMessagesTo("audit2");

        var highPriorityEndpoint = await Endpoint.Start(highPriorityEndpointConfig).ConfigureAwait(false);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            //var sendOption = new SendOptions();
            //sendOption.SetDestination("Samples.ComplexSagaFindingLogic.HighPriority");

            //await endpointInstance.Send(new HighPriorityMessage { Prop = "High Prio" }, sendOption).ConfigureAwait(false);
            //await endpointInstance.Send("Samples.ComplexSagaFindingLogic", new HighPriorityMessage { Prop = "Normal" }).ConfigureAwait(false);


            //await endpointInstance.SendLocal(new StartOrder
            //              {
            //                  OrderId = "123"
            //              })
            //              .ConfigureAwait(false);
            //await endpointInstance.SendLocal(new StartOrder
            //              {
            //                  OrderId = "456"
            //              })
            //              .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}


public class HighPriorityHandler : IHandleMessages<HighPriorityMessage>
{
    public Task Handle(HighPriorityMessage message, IMessageHandlerContext context)
    {
        return Task.FromResult(true);
    }
}

public class Check : CustomCheck
{
    public Check() : base("Custom Check 1", "Custom categort", null)
    {
    }

    public override Task<CheckResult> PerformCheck()
    {
        var result = new CheckResult() { };

        return Task.FromResult(result);
    }
}