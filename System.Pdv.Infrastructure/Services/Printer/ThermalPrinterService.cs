using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Runtime.Versioning;
using System.Management;
using System.Pdv.Core.Enums;

namespace System.Pdv.Infrastructure.Services.Printer;

public class ThermalPrinterService : IThermalPrinterService
{
    private readonly PrintDocument? _printDocument;
    private List<Pedido>? _pedidos;
    private readonly string? _printerName;

    [SupportedOSPlatform("windows")]
    public ThermalPrinterService(IConfiguration configuration)
    {
        _printerName = configuration.GetSection("ThermalPrinter:PrinterName").Value;

        _printDocument = new PrintDocument
        {
            PrinterSettings = new PrinterSettings
            {
                PrinterName = _printerName
            }
        };

        _printDocument.PrintPage += OnPrintPage;
    }

    [SupportedOSPlatform("windows")]
    public bool PrintOrders(IEnumerable<Pedido> pedidos)
    {
        if (!IsPrinterConnected())
        {
            Console.WriteLine("Impressora desconectada ou offline.");
            return false;
        }

        try
        {
            _pedidos = pedidos.ToList();
            _printDocument.Print();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao imprimir: {ex.Message}");
            return false;
        }
    }

    [SupportedOSPlatform("windows")]
    public void ListAvailablePrinters()
    {
        foreach (string printer in PrinterSettings.InstalledPrinters)
        {
            Console.WriteLine($"Impressora disponível: {printer}");
        }
    }

    [SupportedOSPlatform("windows")]
    private void OnPrintPage(object sender, PrintPageEventArgs e)
    {
        var graphics = e.Graphics;
        var fontRegular = new Font("Arial", 9);
        var fontBold = new Font("Arial", 10, FontStyle.Bold);
        var lineHeight = fontRegular.GetHeight();
        var margin = 5f;
        var y = margin;

        Pedido? pedidoAnterior = null;

        foreach (var pedido in _pedidos)
        {
            // Se o pedido anterior for nulo, o cliente mudar ou o tipo de pedido mudar (Interno/Externo), imprimir novo cabeçalho
            if (pedidoAnterior == null ||
                pedidoAnterior.Cliente.Id != pedido.Cliente.Id || 
                pedidoAnterior.TipoPedido != pedido.TipoPedido)
            {
                // Imprimir cabeçalho
                graphics?.DrawString("********** PEDIDO **********", fontBold, Brushes.Black, new PointF(margin, y));
                y += lineHeight + 5;

                if (pedido.TipoPedido == TipoPedido.Interno && pedido.MesaId.HasValue)
                {
                    graphics?.DrawString($"Mesa: {pedido.Mesa.Numero}", fontRegular, Brushes.Black, new PointF(margin, y));
                    y += lineHeight;
                }

                graphics?.DrawString($"Tipo de Pedido: {pedido.TipoPedido}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;
                graphics?.DrawString($"Cliente: {pedido.Cliente.Nome}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;
                graphics?.DrawString($"Telefone: {pedido.Cliente.Telefone}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;
                graphics?.DrawString($"Garçom: {pedido.Garcom.Nome}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;
                graphics?.DrawString($"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight + 5;

                // Itens dos pedidos
                graphics?.DrawString("** Itens dos Pedidos **", fontBold, Brushes.Black, new PointF(margin, y));
                y += lineHeight + 5;
            }

            // Imprimir itens do pedido
            foreach (var item in pedido.Items)
            {
                graphics?.DrawString($"- {item.Produto.Nome} (x{item.Quantidade})", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;

                if (!string.IsNullOrWhiteSpace(item.Observacoes))
                {
                    graphics?.DrawString($"Observação: {item.Observacoes}", fontRegular, Brushes.Black, new PointF(margin + 10, y));
                    y += lineHeight;
                }

                if (item.Adicionais.Any())
                {
                    graphics?.DrawString("-- Adicionais:", fontRegular, Brushes.Black, new PointF(margin + 10, y));
                    y += lineHeight;

                    foreach (var adicional in item.Adicionais)
                    {
                        graphics?.DrawString($"  - {adicional.Nome}", fontRegular, Brushes.Black, new PointF(margin + 20, y));
                        y += lineHeight;
                    }
                }

                graphics?.DrawString("----------------------------------------------", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;
            }

            // Atualizar pedido anterior
            pedidoAnterior = pedido;
        }

        y += 5;
        graphics?.DrawString("**** FIM DO PEDIDO ****", fontBold, Brushes.Black, new PointF(margin, y));
        e.HasMorePages = false;
    }

    //Para verificar se a impressora termica ta online
    [SupportedOSPlatform("windows")]
    public bool IsPrinterConnected()
    {
        var printerSearcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_Printer");
        foreach (var printer in printerSearcher.Get())
        {
            var printerName = printer["Name"].ToString();

            //Se a impressora tiver offline vai retornar true
            var isPrinterOnline = (bool)printer["WorkOffline"] == false;

            if (printerName.Equals(_printerName, StringComparison.OrdinalIgnoreCase) && isPrinterOnline)
            {
                return true;
            }
        }

        return false;
    }
}
