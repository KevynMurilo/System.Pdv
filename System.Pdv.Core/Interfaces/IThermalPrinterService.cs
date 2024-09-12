using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IThermalPrinterService
{
    bool PrintOrders(IEnumerable<Pedido> pedidos);
}
