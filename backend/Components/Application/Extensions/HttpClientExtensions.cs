using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Application.Extensions;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder AddTransientHttpErrorPolicy(this IHttpClientBuilder builder)
    {
        return builder.AddTransientHttpErrorPolicy(option =>
        {
            return option.WaitAndRetryAsync(6,
                (retryAttempts) => TimeSpan.FromSeconds(Math.Pow(2f, retryAttempts)));
        });
    }
}
