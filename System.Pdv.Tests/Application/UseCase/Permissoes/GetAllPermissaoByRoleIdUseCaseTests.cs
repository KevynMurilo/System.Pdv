using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Application.UseCase.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Tests.UseCase.Permissoes;

public class GetAllPermissaoByRoleIdUseCaseTests
{
    private readonly Mock<IPermissaoRepository> _mockPermissaoRepository;
    private readonly Mock<ILogger<GetAllPermissaoByRoleIdUseCase>> _mockLogger;
    private readonly GetAllPermissaoByRoleIdUseCase _useCase;

    public GetAllPermissaoByRoleIdUseCaseTests()
    {
        _mockPermissaoRepository = new Mock<IPermissaoRepository>();
        _mockLogger = new Mock<ILogger<GetAllPermissaoByRoleIdUseCase>>();
        _useCase = new GetAllPermissaoByRoleIdUseCase(
            _mockPermissaoRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnPermissions_WhenPermissionsExist()
    {
        var roleId = Guid.NewGuid();
        var permissoes = new List<Permissao>
        {
            new Permissao { Id = Guid.NewGuid(), Recurso = "Resource1", Acao = "Action1" },
            new Permissao { Id = Guid.NewGuid(), Recurso = "Resource2", Acao = "Action2" }
        };

        _mockPermissaoRepository.Setup(p => p.GetAllPermissaoByRoleIdAsync(roleId)).ReturnsAsync(permissoes);

        var result = await _useCase.ExecuteAsync(roleId);

        Assert.NotNull(result.Result);
        Assert.Equal(permissoes.Count, result.Result.Count());
        Assert.True(result.ReqSuccess);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenNoPermissionsExist()
    {
        var roleId = Guid.NewGuid();

        _mockPermissaoRepository.Setup(p => p.GetAllPermissaoByRoleIdAsync(roleId)).ReturnsAsync(new List<Permissao>());

        var result = await _useCase.ExecuteAsync(roleId);

        Assert.Null(result.Result);
        Assert.Equal($"Nenhuma permissão encontrada com o Id - {roleId}", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        var roleId = Guid.NewGuid();

        _mockPermissaoRepository.Setup(p => p.GetAllPermissaoByRoleIdAsync(roleId)).ThrowsAsync(new Exception("Database error"));

        var result = await _useCase.ExecuteAsync(roleId);

        Assert.False(result.ReqSuccess);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
