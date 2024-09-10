using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IThermalPrinterService
{
    void PrintOrder(Pedido pedido);
}
