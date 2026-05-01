using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitMessaging(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                config.AddConsumers(assembly);
            }
            
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            
            });
        });

        return services;
    }
}