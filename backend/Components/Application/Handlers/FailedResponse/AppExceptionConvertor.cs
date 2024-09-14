
using SharedKernel.Exceptions;

namespace Application.Handlers.FailedResponse;

public class AppExceptionConvertor : IExceptionConvertor
{
    public FailedResponseModel? ToFailedResponse(Exception ex)
    {
        if (ex is AppException appEx)
        {
            if (appEx.StatusCode is not null)
            {
                return new FailedResponseModel((int)appEx.StatusCode, appEx.Message);
            }
        }
        return null;
    }
}
