using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IThermalPrinterService
{
    bool PrintOrder(Pedido pedido);
}
