using Application.Handlers.FailedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder ConfigureInvalidModelStateResponse(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = (context) =>
            {
                var errors = context.ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(x => x.Key, x => "Invalid value");
                return new BadRequestObjectResult(
                    new FailedResponseModel(StatusCodes.Status400BadRequest, "Invalid request", errors));
            };
        });
    }
}
