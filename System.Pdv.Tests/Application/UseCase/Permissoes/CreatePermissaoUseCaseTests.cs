using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;
using System.Pdv.Application.UseCase.Permissoes;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Tests.UseCase.Permissoes;

public class CreatePermissaoUseCaseTests
{
    private readonly Mock<IPermissaoRepository> _mockPermissaoRepository;
    private readonly Mock<ILogger<CreatePermissaoUseCase>> _mockLogger;
    private readonly CreatePermissaoUseCase _useCase;

    public CreatePermissaoUseCaseTests()
    {
        _mockPermissaoRepository = new Mock<IPermissaoRepository>();
        _mockLogger = new Mock<ILogger<CreatePermissaoUseCase>>();
        _useCase = new CreatePermissaoUseCase(
            _mockPermissaoRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenPermissionIsCreated()
    {
        var permissionDto = new CreatePermissionDto { Recurso = "TestResource", Acao = "TestAction" };
        var permissao = new Permissao
        {
            Id = Guid.NewGuid(),
            Recurso = permissionDto.Recurso,
            Acao = permissionDto.Acao
        };

        _mockPermissaoRepository.Setup(p => p.AddAsync(It.IsAny<Permissao>())).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.NotNull(result.Result);
        Assert.Equal(permissionDto.Recurso, result.Result.Recurso);
        Assert.Equal(permissionDto.Acao, result.Result.Acao);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        var permissionDto = new CreatePermissionDto { Recurso = "TestResource", Acao = "TestAction" };

        _mockPermissaoRepository.Setup(p => p.AddAsync(It.IsAny<Permissao>())).ThrowsAsync(new Exception("Database error"));

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.False(result.ServerOn);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
