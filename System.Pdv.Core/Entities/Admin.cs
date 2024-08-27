namespace System.Pdv.Core.Entities;

public class Admin
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
