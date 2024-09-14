using Application.Handlers;
using Application.Handlers.Identities;
using Domain.Events.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddDomainEventsAccessor<TEventAccessor>(
        this IServiceCollection services) where TEventAccessor : class, IDomainEventsAccessor
    {
        services.TryAddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.TryAddScoped<IDomainEventsAccessor, TEventAccessor>();

        return services;
    }

    public static IServiceCollection AddBearerTokenHandler(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ClientCredentials> configureClientCredentials)
    {
        services.Configure<ClientCredentials>(configureClientCredentials)
            .AddTransient<BearerTokenHandler>()
            .AddMemoryCache();

        services.AddHttpClient("BearerTokenHandlerClient")
            .AddTransientHttpErrorPolicy();

        return services;
    }
}
