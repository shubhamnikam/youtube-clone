namespace Application.Handlers.FailedResponse;

public interface IExceptionConvertor
{
    FailedResponseModel? ToFailedResponse(Exception ex);
}
