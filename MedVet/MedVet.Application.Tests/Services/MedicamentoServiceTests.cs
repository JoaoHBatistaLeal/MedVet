using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Implementations;
using MedVet.Domain.Entities;
using Moq;

namespace MedVet.Application.Tests.Services;

public class MedicamentoServiceTests
{
    private readonly Mock<IRepository<Medicamento>> _medicamentoRepository = new();
    private readonly MedicamentoService _medicamentoService;

    public MedicamentoServiceTests()
    {
        _medicamentoService = new MedicamentoService(_medicamentoRepository.Object);
    }

    [Fact]
    public void Create_ComDadosValidos_DeveChamarRepositorioGenericoERetornarResponse()
    {
        var request = new MedicamentoRequest("Anti-inflamatorio", "VetPharma", "Comprimido", 30.0);

        var response = _medicamentoService.Create(request);

        Assert.NotNull(response);
        Assert.Equal("Anti-inflamatorio", response.NomeMedicamento);
        Assert.Equal(30.0, response.Preco);
        _medicamentoRepository.Verify(r => r.Add(It.IsAny<Medicamento>()), Times.Once);
    }

    [Fact]
    public void GetById_QuandoNaoExiste_DeveRetornarNull()
    {
        var id = Guid.NewGuid();
        _medicamentoRepository.Setup(r => r.GetById(id)).Returns((Medicamento?)null);

        var result = _medicamentoService.GetById(id);

        Assert.Null(result);
        _medicamentoRepository.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public void GetPaged_ComParametrosValidos_DeveChamarRepositorioGenericoERetornarItensETotal()
    {
        var medicamentos = new List<Medicamento>
        {
            new("Amoxicilina", "VetCare", "Comprimido", 45.0),
            new("Dipirona", "VetCare", "Gotas", 20.0)
        };
        _medicamentoRepository.Setup(r => r.GetPaged(1, 10)).Returns((medicamentos, 2));

        var (items, total) = _medicamentoService.GetPaged(1, 10);

        Assert.Equal(2, total);
        Assert.Equal(2, items.Count);
        Assert.Equal("Amoxicilina", items[0].NomeMedicamento);
        Assert.Equal("Dipirona", items[1].NomeMedicamento);
        _medicamentoRepository.Verify(r => r.GetPaged(1, 10), Times.Once);
    }
}
