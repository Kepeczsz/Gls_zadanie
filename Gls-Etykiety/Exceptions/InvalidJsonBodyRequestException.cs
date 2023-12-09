namespace Gls_Etykiety.Exceptions;

public class InvalidJsonBodyRequestException : Exception
{
    public InvalidJsonBodyRequestException()
    : base()
        {
        }

    public InvalidJsonBodyRequestException(string message)
        : base(message)
    {
    }

    public InvalidJsonBodyRequestException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
