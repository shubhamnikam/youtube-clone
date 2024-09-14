namespace Application.Handlers.FailedResponse;
public class HttpRequestExceptionConvertor : IExceptionConvertor
{
    public FailedResponseModel? ToFailedResponse(Exception ex)
    {
        if (ex is HttpRequestException appEx)
        {
            if (appEx.StatusCode != null)
            {
                return new FailedResponseModel((int)appEx.StatusCode, appEx.Message);
            }
        }

        return null;
    }
}