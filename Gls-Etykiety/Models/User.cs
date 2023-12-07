namespace Gls_Etykiety.Models;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string username { get; private set; } = default!;

    public string password { get; private set; } = default!;
}
