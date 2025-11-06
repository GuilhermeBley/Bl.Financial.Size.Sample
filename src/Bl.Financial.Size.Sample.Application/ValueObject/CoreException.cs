namespace Bl.Financial.Size.Sample.Application.ValueObject;

public class CoreException : Exception
{
    public override string Source => "Bl.Financial.Size";
    public int Code { get; } = 400;

    public CoreException()
    {
    }

    public CoreException(string? message) : base(message)
    {
    }

    public CoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public CoreException(int code, string? message) : base(message)
    {
        Code = code;
    }

    public CoreException(int code, string? message, Exception? innerException) : base(message, innerException)
    {
        Code = code;
    }
}
