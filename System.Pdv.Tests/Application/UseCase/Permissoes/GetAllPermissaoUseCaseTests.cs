using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Application.UseCase.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Tests.UseCase.Permissoes;

public class GetAllPermissaoUseCaseTests
{
    private readonly Mock<IPermissaoRepository> _mockPermissaoRepository;
    private readonly Mock<ILogger<GetAllPermissaoUseCase>> _mockLogger;
    private readonly GetAllPermissaoUseCase _useCase;

    public GetAllPermissaoUseCaseTests()
    {
        _mockPermissaoRepository = new Mock<IPermissaoRepository>();
        _mockLogger = new Mock<ILogger<GetAllPermissaoUseCase>>();
        _useCase = new GetAllPermissaoUseCase(
            _mockPermissaoRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnPermissions_WhenPermissionsExist()
    {
        var permissoes = new List<Permissao>
        {
            new Permissao { Id = Guid.NewGuid(), Recurso = "Resource1", Acao = "Action1" },
            new Permissao { Id = Guid.NewGuid(), Recurso = "Resource2", Acao = "Action2" }
        };

        _mockPermissaoRepository.Setup(p => p.GetAllPermissao(1, 10, null, null)).ReturnsAsync(permissoes);

        var result = await _useCase.ExecuteAsync(1, 10, null, null);

        Assert.NotNull(result.Result);
        Assert.Equal(permissoes.Count, result.Result.Count());
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenNoPermissionsExist()
    {
        _mockPermissaoRepository.Setup(p => p.GetAllPermissao(1, 10, null, null)).ReturnsAsync(new List<Permissao>());

        var result = await _useCase.ExecuteAsync(1, 10, null, null);

        Assert.Null(result.Result);
        Assert.Equal("Nenhuma permissão encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        _mockPermissaoRepository.Setup(p => p.GetAllPermissao(1, 10, null, null)).ThrowsAsync(new Exception("Database error"));

        var result = await _useCase.ExecuteAsync(1, 10, null, null);

        Assert.False(result.ReqSuccess);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
