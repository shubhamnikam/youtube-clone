using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Identities;

public class BearerTokenHandler : DelegatingHandler
{
    private readonly ClientCredentials clientCredentials;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IMemoryCache memoryCache;
    private readonly IHostingEnvironment env;
    private readonly ILogger<BearerTokenHandler> logger;

    public BearerTokenHandler(IOptions<ClientCredentials> clientCredentials, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, IHostingEnvironment env, ILogger<BearerTokenHandler> logger)
    {
        this.clientCredentials = clientCredentials.Value;
        this.httpClientFactory = httpClientFactory;
        this.memoryCache = memoryCache;
        this.env = env;
        this.logger = logger;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = GetAccessToken().ConfigureAwait(false).GetAwaiter().GetResult();

        if (accessToken == null)
        {
            request.SetBearerToken(accessToken);
        }
        return base.Send(request, cancellationToken);
    }

    private async Task<string?> GetAccessToken()
    {
        string cacheId = $"Token:{clientCredentials?.ClientId}";

        if (memoryCache.TryGetValue<string>(cacheId, out var token))
        {
            return token;
        }
        else
        {
            using var httpClient = httpClientFactory.CreateClient("BearerTokenHandlerClient");
            var discovery = await httpClient.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest()
                {
                    Address = clientCredentials.Authority,
                    Policy = new DiscoveryPolicy()
                    {
                        RequireHttps = !env.IsDevelopment()
                    }
                });

            if (discovery.IsError)
            {
                logger.LogError("Failed to get discovery document from {Authority}", clientCredentials.Authority);
                return null;
            }

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discovery.TokenEndpoint,
                    ClientId = clientCredentials.ClientId,
                    ClientSecret = clientCredentials.ClientSecret,
                    Scope = clientCredentials.Scope
                }
            );

            if (tokenResponse.IsError || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                logger.LogError("Failed to get token response from {Address}", discovery.TokenEndpoint);
                return null;
            }

            logger.LogInformation("Obtained acces Token: {AccessToken}", tokenResponse.AccessToken);

            logger.LogInformation("Cache token response for {ClientId}, expired in {ExpiredIn}",
                clientCredentials.ClientId, tokenResponse.ExpiresIn);

            using (var entry = memoryCache.CreateEntry(cacheId))
            {
                entry.SetValue(tokenResponse.AccessToken);
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 60));
                entry.SetPriority(CacheItemPriority.NeverRemove);
            }

            return tokenResponse.AccessToken;
        }
    }
}
