using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Implementations;
using MedVet.Domain.Entities;
using Moq;

namespace MedVet.Application.Tests.Services;

public class PetServiceTests
{
    private readonly Mock<IPetRepository> _petRepository = new();
    private readonly Mock<IDonoRepository> _donoRepository = new();
    private readonly PetService _petService;

    public PetServiceTests()
    {
        _petService = new PetService(_petRepository.Object, _donoRepository.Object);
    }

    [Fact]
    public void Create_QuandoDonoNaoExiste_DeveLancarExceptionENaoPersistir()
    {
        var request = new PetRequest("Rex", "Cachorro", "Beagle", "Macho", Guid.NewGuid());
        _donoRepository.Setup(r => r.GetById(request.IdDono)).Returns((Dono?)null);

        var act = () => _petService.Create(request);

        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Dono não encontrado.", ex.Message);
        _petRepository.Verify(r => r.Add(It.IsAny<Pet>()), Times.Never);
        _petRepository.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Create_QuandoDonoExiste_DevePersistirERetornarPetResponse()
    {
        var dono = new Dono("Maria", "maria@email.com", "11999998888");
        var request = new PetRequest("Thor", "Cachorro", "Golden", "Macho", dono.Id);
        _donoRepository.Setup(r => r.GetById(dono.Id)).Returns(dono);

        var response = _petService.Create(request);

        Assert.NotNull(response);
        Assert.Equal("Thor", response.Nome);
        Assert.Equal(dono.Id, response.IdDono);
        _petRepository.Verify(r => r.Add(It.IsAny<Pet>()), Times.Once);
        _petRepository.Verify(r => r.SaveChanges(), Times.Once);
    }
}
