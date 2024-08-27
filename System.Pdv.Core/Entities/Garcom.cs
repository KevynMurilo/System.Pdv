namespace System.Pdv.Core.Entities;

public class Garcom
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<Pedido> Pedidos { get; set; }
}
