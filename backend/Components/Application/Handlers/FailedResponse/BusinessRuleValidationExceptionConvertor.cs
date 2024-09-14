using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.FailedResponse;
public class BusinessRuleValidationExceptionConvertor : IExceptionConvertor
{
    public FailedResponseModel? ToFailedResponse(Exception ex)
    {
        if (ex is BusinessRuleValidationException ruleEx)
        {
            return new FailedResponseModel(StatusCodes.Status400BadRequest, ruleEx.Message);
        }

        return null;
    }
}