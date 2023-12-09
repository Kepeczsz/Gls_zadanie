using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gls_Etykiety.Exceptions;

public class GlsApiException : Exception
{
    public GlsApiException()
: base()
    {
    }

    public GlsApiException(string message)
        : base(message)
    {
    }

    public GlsApiException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
