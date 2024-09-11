using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Runtime.Versioning;
using System.Management;

namespace System.Pdv.Infrastructure.Services
{
    public class ThermalPrinterService : IThermalPrinterService
    {
        private readonly PrintDocument _printDocument;
        private Pedido _pedido;
        private readonly string _printerName;

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
        public bool PrintOrder(Pedido pedido)
        {
            if (!IsPrinterConnected())
            {
                Console.WriteLine("Impressora desconectada ou offline.");
                return false;
            }

            try
            {
                _pedido = pedido;
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

            graphics?.DrawString("**** PEDIDO ****", fontBold, Brushes.Black, new PointF(margin, y));
            y += lineHeight + 5;

            graphics?.DrawString($"ID: {_pedido.Id}", fontRegular, Brushes.Black, new PointF(margin, y));
            y += lineHeight;

            if (_pedido.TipoPedido == TipoPedido.Interno && _pedido.MesaId.HasValue)
            {
                graphics?.DrawString($"Tipo Pedido: {_pedido.TipoPedido}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;

                graphics?.DrawString($"Mesa: {_pedido.Mesa.Numero}", fontRegular, Brushes.Black, new PointF(margin, y));
                y += lineHeight;
            }

            graphics?.DrawString($"Cliente: {_pedido.Cliente.Nome}", fontRegular, Brushes.Black, new PointF(margin, y));
            y += lineHeight;

            graphics?.DrawString($"Telefone: {_pedido.Cliente.Telefone}", fontRegular, Brushes.Black, new PointF(margin, y));
            y += lineHeight;

            graphics?.DrawString($"Garçom: {_pedido.Garcom.Nome}", fontRegular, Brushes.Black, new PointF(margin, y));
            y += lineHeight;

            graphics?.DrawString($"Data: {_pedido.DataHora:dd/MM/yyyy HH:mm:ss}", fontRegular, Brushes.Black, new PointF(margin, y));
            y += lineHeight + 5;

            graphics?.DrawString("** Itens do Pedido **", fontBold, Brushes.Black, new PointF(margin, y));
            y += lineHeight + 5;

            foreach (var item in _pedido.Items)
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

            y += 5;
            graphics?.DrawString("**** FIM DO PEDIDO ****", fontBold, Brushes.Black, new PointF(margin, y));
            e.HasMorePages = false;
        }

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
}
