using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gls_Etykiety.Models;

public class Label
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public byte[] Data { get; set; } = default!;
}
