namespace Gls_Etykiety.Exceptions;

public class NoDataFoundException :Exception
{
    public NoDataFoundException()
    : base()
    {
    }

    public NoDataFoundException(string message)
        : base(message)
    {
    }

    public NoDataFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
