using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Services.Adicionais;
public class CreateAdicionalServiceTests
{
    private readonly Mock<IAdicionalRepository> _adicionalRepositoryMock;
    private readonly Mock<ILogger<CreateAdicionalService>> _loggerMock;
    private readonly CreateAdicionalService _createAdicionalService;

    public CreateAdicionalServiceTests()
    {
        _adicionalRepositoryMock = new Mock<IAdicionalRepository>();
        _loggerMock = new Mock<ILogger<CreateAdicionalService>>();
        _createAdicionalService = new CreateAdicionalService(_adicionalRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAdicional_ShouldReturnConflict_WhenAdicionalAlreadyExists()
    {
        var adicionalDto = new AdicionalDto { Nome = "Adicional Teste", Preco = 10.0m };
        _adicionalRepositoryMock.Setup(repo => repo.GetByNameAsync(adicionalDto.Nome))
            .ReturnsAsync(new ItemAdicional());

        var result = await _createAdicionalService.CreateAdicional(adicionalDto);

        Assert.Equal(409, result.StatusCode);
        Assert.Equal("Adicional já registrado", result.Message);
    }

    [Fact]
    public async Task CreateAdicional_ShouldReturnSuccess_WhenAdicionalIsCreatedSuccessfully()
    {
        var adicionalDto = new AdicionalDto { Nome = "Novo Adicional", Preco = 10.0m };
        _adicionalRepositoryMock.Setup(repo => repo.GetByNameAsync(adicionalDto.Nome))
            .ReturnsAsync((ItemAdicional)null);

        _adicionalRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ItemAdicional>()))
            .Returns(Task.CompletedTask);

        var result = await _createAdicionalService.CreateAdicional(adicionalDto);

        Assert.NotNull(result.Result);
        Assert.Equal(adicionalDto.Nome.ToUpper(), result.Result.Nome);
        Assert.Equal(adicionalDto.Preco, result.Result.Preco);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task CreateAdicional_ShouldLogErrorAndReturnError_WhenExceptionIsThrown()
    {
        var adicionalDto = new AdicionalDto { Nome = "Novo Adicional", Preco = 10.0m };
        var exception = new Exception("Database error");

        _adicionalRepositoryMock.Setup(repo => repo.GetByNameAsync(adicionalDto.Nome))
            .ThrowsAsync(exception);

        var result = await _createAdicionalService.CreateAdicional(adicionalDto);

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
    }
}
