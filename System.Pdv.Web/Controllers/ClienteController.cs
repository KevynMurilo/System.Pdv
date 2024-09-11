using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.Interfaces.Clientes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IGetAllClienteUseCase _getAllClienteUseCase;
    private readonly IGetByIdClienteUseCase _getByIdClienteUseCase;
    private readonly IGetByNameClienteUseCase _getByNameClienteUseCase;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(
        IGetAllClienteUseCase getAllClienteUseCase,
        IGetByIdClienteUseCase getByIdClienteUseCase,
        IGetByNameClienteUseCase getByNameClienteUseCase,
        ILogger<ClienteController> logger)
    {
        _getAllClienteUseCase = getAllClienteUseCase;
        _getByIdClienteUseCase = getByIdClienteUseCase;
        _getByNameClienteUseCase = getByNameClienteUseCase;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClientes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllClienteUseCase.ExecuteAsync(pageNumber, pageSize);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar clientes");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("nome")]
    public async Task<IActionResult> GetByName([FromQuery] string nome)
    {
        try
        {
            var result = await _getByNameClienteUseCase.ExecuteAsync(nome);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar cliente por nome");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdCliente(Guid id)
    {
        try
        {
            var result = await _getByIdClienteUseCase.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar cliente por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
