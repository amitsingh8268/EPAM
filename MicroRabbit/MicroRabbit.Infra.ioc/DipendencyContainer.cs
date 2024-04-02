using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infa.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.Ioc
{
    public class DipendencyContainer
    {
        public static void registerServices(IServiceCollection services)
        {
            // domain bus
            services.AddTransient<IEventBus, RabbitMQBus>();
        }
    }
}
