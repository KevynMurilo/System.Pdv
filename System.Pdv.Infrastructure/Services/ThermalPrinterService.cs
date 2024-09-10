using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Infrastructure.Services
{
    public class ThermalPrinterService : IThermalPrinterService
    {
        private readonly PrintDocument _printDocument;
        private Pedido _pedido;

        public ThermalPrinterService(IConfiguration configuration)
        {
            var printerName = configuration.GetSection("ThermalPrinter:PrinterName").Value;

            _printDocument = new PrintDocument
            {
                PrinterSettings = new PrinterSettings
                {
                    PrinterName = printerName
                }
            };

            _printDocument.PrintPage += OnPrintPage;
        }

        public void PrintOrder(Pedido pedido)
        {
            _pedido = pedido;
            _printDocument.Print();
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            var graphics = e.Graphics;
            var font = new Font("Arial", 10);
            var lineHeight = font.GetHeight();

            var y = 0f;

            // Informações básicas do pedido
            graphics.DrawString("==== PEDIDO ====", font, Brushes.Black, new PointF(0, y));
            y += lineHeight;

            graphics.DrawString($"ID: {_pedido.Id}", font, Brushes.Black, new PointF(0, y));
            y += lineHeight;

            // Verifica se o pedido é interno e imprime o ID da mesa
            if (_pedido.TipoPedido == TipoPedido.Interno && _pedido.MesaId.HasValue)
            {
                graphics.DrawString($"Mesa: {_pedido.MesaId}", font, Brushes.Black, new PointF(0, y));
                y += lineHeight;
            }

            graphics.DrawString($"Cliente: {_pedido.Cliente.Nome}", font, Brushes.Black, new PointF(0, y));
            y += lineHeight;

            graphics.DrawString($"Garçom: {_pedido.Garcom.Nome}", font, Brushes.Black, new PointF(0, y));
            y += lineHeight;

            graphics.DrawString($"Data: {_pedido.DataHora.ToString("dd/MM/yyyy HH:mm:ss")}", font, Brushes.Black, new PointF(0, y));
            y += lineHeight;

            // Imprimir os itens do pedido
            foreach (var item in _pedido.Items)
            {
                graphics.DrawString($"Produto: {item.Produto.Nome}", font, Brushes.Black, new PointF(0, y));
                y += lineHeight;

                graphics.DrawString($"Quantidade: {item.Quantidade}", font, Brushes.Black, new PointF(0, y));
                y += lineHeight;

                graphics.DrawString($"Observação: R$ {item.Observacoes}", font, Brushes.Black, new PointF(0, y));
                y += lineHeight;

                graphics.DrawString($"Preço: R$ {item.Produto.Preco:F2}", font, Brushes.Black, new PointF(0, y));
                y += lineHeight;

                // Imprimir adicionais se houver
                if (item.Adicionais.Any())
                {
                    graphics.DrawString("Adicionais:", font, Brushes.Black, new PointF(0, y));
                    y += lineHeight;
                    foreach (var adicional in item.Adicionais)
                    {
                        graphics.DrawString($"- {adicional.Nome}: R$ {adicional.Preco:F2}", font, Brushes.Black, new PointF(0, y));
                        y += lineHeight;
                    }
                }

                // Imprimir observações se houver
                if (!string.IsNullOrWhiteSpace(item.Observacoes))
                {
                    graphics.DrawString($"Observações: {item.Observacoes}", font, Brushes.Black, new PointF(0, y));
                    y += lineHeight;
                }

                graphics.DrawString("------------------------", font, Brushes.Black, new PointF(0, y));
                y += lineHeight;
            }

            graphics.DrawString("====================", font, Brushes.Black, new PointF(0, y));
            y += lineHeight;

            // A impressão será finalizada automaticamente
            e.HasMorePages = false;
        }
    }
}
