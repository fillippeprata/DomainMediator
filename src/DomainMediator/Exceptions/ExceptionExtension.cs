namespace DomainMediator.Exceptions;

public static class ExceptionExtension
{
    public static string RootExceptionText(this Exception ex)
    {
        return ex.InnerException == null ? ex.Message : $"{ex.Message} -> {ex.InnerException.RootExceptionText()}";
    }
}