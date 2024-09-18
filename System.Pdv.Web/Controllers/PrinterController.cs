using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace System.Pdv.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PrinterController> _logger;
        private const string ConfigSection = "ThermalPrinter:PrinterName";

        public PrinterController(IConfiguration configuration, ILogger<PrinterController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HasPermission("Printer", "Get")]
        [HttpGet("disponiveis")]
        public IActionResult GetAvailablePrinters()
        {
            try
            {
                var printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
                if (!printers.Any()) return NotFound("Nenhuma impressora encontrada");

                return Ok(printers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar impressoras disponíveis.");
                return StatusCode(500, "Ocorreu um erro inesperado ao listar impressoras.");
            }
        }

        [HasPermission("Printer", "Create")]
        [HttpPost("selecionar")]
        public IActionResult SelectPrinter([FromBody] string printerName)
        {
            try
            {
                if (!PrinterSettings.InstalledPrinters.Cast<string>().Contains(printerName))
                    return BadRequest("Impressora não encontrada.");
                

                UpdatePrinterConfig(printerName);
                return Ok($"Impressora {printerName} selecionada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao selecionar impressora.");
                return StatusCode(500, "Ocorreu um erro inesperado ao selecionar a impressora.");
            }
        }

        [HasPermission("Printer", "Get")]
        [HttpGet("selecionada")]
        public IActionResult GetSelectedPrinter()
        {
            try
            {
                var selectedPrinter = _configuration[ConfigSection];
                return Ok(selectedPrinter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a impressora selecionada.");
                return StatusCode(500, "Ocorreu um erro inesperado ao recuperar a impressora selecionada.");
            }
        }

        private void UpdatePrinterConfig(string printerName)
        {
            try
            {
                var configFile = "appsettings.json";
                var json = IO.File.ReadAllText(configFile);
                dynamic? config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                config["ThermalPrinter"]["PrinterName"] = printerName;
                string updatedJson = Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);
                IO.File.WriteAllText(configFile, updatedJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a configuração da impressora.");
                throw;
            }
        }
    }
}
