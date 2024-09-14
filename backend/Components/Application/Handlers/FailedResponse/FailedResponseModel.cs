namespace Application.Handlers.FailedResponse;

public class FailedResponseModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public IDictionary<string, string>? Errors { get; set; }

    public FailedResponseModel(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
    public FailedResponseModel(int statusCode, string message, IDictionary<string, string>? errors)
        : this(statusCode, message)
    {
        Errors = errors;
    }
}
