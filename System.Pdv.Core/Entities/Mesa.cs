using System.Pdv.Core.Enums;

namespace System.Pdv.Core.Entities;

public class Mesa
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public StatusMesa Status { get; set; } = StatusMesa.Livre;
}
