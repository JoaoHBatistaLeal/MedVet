using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Implementations;
using MedVet.Domain.Entities;
using Moq;

namespace MedVet.Application.Tests.Services;

public class ConsultaServiceTests
{
    private readonly Mock<IConsultaRepository> _consultaRepository = new();
    private readonly Mock<IPetRepository> _petRepository = new();
    private readonly Mock<IVeterinarioRepository> _veterinarioRepository = new();
    private readonly ConsultaService _consultaService;

    public ConsultaServiceTests()
    {
        _consultaService = new ConsultaService(
            _consultaRepository.Object,
            _petRepository.Object,
            _veterinarioRepository.Object);
    }

    [Fact]
    public void Create_QuandoPetNaoExiste_DeveLancarExceptionENaoPersistir()
    {
        var request = new ConsultaRequest(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "Exame de rotina", "Tudo normal");
        _petRepository.Setup(r => r.GetById(request.IdPet)).Returns((Pet?)null);

        var act = () => _consultaService.Create(request);

        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Pet não encontrado.", ex.Message);
        _consultaRepository.Verify(r => r.Add(It.IsAny<Consulta>()), Times.Never);
        _consultaRepository.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Create_QuandoVeterinarioNaoExiste_DeveLancarExceptionENaoPersistir()
    {
        var pet = new Pet("Bob", "Cachorro", "Poodle", "Macho", Guid.NewGuid());
        var request = new ConsultaRequest(pet.Id, Guid.NewGuid(), DateTime.Now, "Consulta geral", "Observar alimentacao");
        _petRepository.Setup(r => r.GetById(pet.Id)).Returns(pet);
        _veterinarioRepository.Setup(r => r.GetById(request.IdVeterinario)).Returns((Veterinario?)null);

        var act = () => _consultaService.Create(request);

        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Veterinário não encontrado.", ex.Message);
        _consultaRepository.Verify(r => r.Add(It.IsAny<Consulta>()), Times.Never);
        _consultaRepository.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Create_QuandoDependenciasExistem_DevePersistirERetornarConsultaResponse()
    {
        var pet = new Pet("Bob", "Cachorro", "Poodle", "Macho", Guid.NewGuid());
        var vet = new Veterinario("Dr. Silva", 12345, "Clinica Geral");
        var request = new ConsultaRequest(pet.Id, vet.Id, DateTime.Now, "Vacinacao", "Aplicada vacina V10");
        _petRepository.Setup(r => r.GetById(pet.Id)).Returns(pet);
        _veterinarioRepository.Setup(r => r.GetById(vet.Id)).Returns(vet);

        var response = _consultaService.Create(request);

        Assert.NotNull(response);
        Assert.Equal("Vacinacao", response.Diagnostico);
        Assert.Equal(pet.Id, response.IdPet);
        Assert.Equal(vet.Id, response.IdVeterinario);
        _consultaRepository.Verify(r => r.Add(It.IsAny<Consulta>()), Times.Once);
        _consultaRepository.Verify(r => r.SaveChanges(), Times.Once);
    }
}
