using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace System.Pdv.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private const string ConfigSection = "ThermalPrinter:PrinterName";

        public PrinterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("disponiveis")]
        public IActionResult GetAvailablePrinters()
        {
            var printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            return Ok(printers);
        }

        [HttpPost("selecionar")]
        public IActionResult SelectPrinter([FromBody] string printerName)
        {
            if (!PrinterSettings.InstalledPrinters.Cast<string>().Contains(printerName))
            {
                return BadRequest("Impressora não encontrada.");
            }

            UpdatePrinterConfig(printerName);
            return Ok($"Impressora {printerName} selecionada com sucesso.");
        }

        [HttpGet("selecionada")]
        public IActionResult GetSelectedPrinter()
        {
            var selectedPrinter = _configuration[ConfigSection];
            return Ok(selectedPrinter);
        }

        private void UpdatePrinterConfig(string printerName)
        {
            var configFile = "appsettings.json";
            var json = IO.File.ReadAllText(configFile);
            dynamic? config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            config["ThermalPrinter"]["PrinterName"] = printerName;
            string updatedJson = Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);
            IO.File.WriteAllText(configFile, updatedJson);
        }
    }
}
