using Moq;
using Xunit;
using System;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.Services.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Services.Adicionais;
public class GetAllAdicionalServicesTests
{
    private readonly Mock<IAdicionalRepository> _adicionalRepositoryMock;
    private readonly Mock<ILogger<GetAllAdicionalServices>> _loggerMock;
    private readonly GetAllAdicionalServices _getAllAdicionalServices;

    public GetAllAdicionalServicesTests()
    {
        _adicionalRepositoryMock = new Mock<IAdicionalRepository>();
        _loggerMock = new Mock<ILogger<GetAllAdicionalServices>>();
        _getAllAdicionalServices = new GetAllAdicionalServices(_adicionalRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAdicionais_ShouldReturnNotFound_WhenNoAdicionaisAreFound()
    {
        _adicionalRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<ItemAdicional>());

        var result = await _getAllAdicionalServices.GetAllAdicionais(1, 10);

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum adicional encontrado", result.Message);
    }

    [Fact]
    public async Task GetAllAdicionais_ShouldReturnSuccess_WhenAdicionaisAreFound()
    {
        var adicionais = new List<ItemAdicional>
        {
            new ItemAdicional { Id = Guid.NewGuid(), Nome = "Adicional 1", Preco = 5.0m },
            new ItemAdicional { Id = Guid.NewGuid(), Nome = "Adicional 2", Preco = 10.0m }
        };

        _adicionalRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(adicionais);

        var result = await _getAllAdicionalServices.GetAllAdicionais(1, 10);

        Assert.Equal(adicionais, result.Result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task GetAllAdicionais_ShouldLogErrorAndReturnError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Database error");

        _adicionalRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(exception);

        var result = await _getAllAdicionalServices.GetAllAdicionais(1, 10);

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
    }
}
