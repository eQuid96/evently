namespace Evently.Shared.Domain;

public class EventlyException : Exception
{
    public string RequestName { get; init; }
    public Error? Error { get; init; }
    
    public EventlyException(string requestName, Error? error = null, Exception? innerException = null) 
        : base("Application exception", innerException: innerException)
    {
        RequestName = requestName;
        Error = error;
    }
}
