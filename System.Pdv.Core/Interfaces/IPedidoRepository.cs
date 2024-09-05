using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido> AddAsync(Pedido pedido);
}
