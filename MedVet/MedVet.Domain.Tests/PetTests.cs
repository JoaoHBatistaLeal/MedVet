using MedVet.Domain.Entities;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Tests;

public class PetTests
{
    [Fact]
    public void Pet_DadosValidos_DeveInstanciarComSucesso()
    {
        var idDono = Guid.NewGuid();
        var pet = new Pet("Rex", "Cachorro", "Labrador", "Macho", idDono);

        Assert.Equal("Rex", pet.Nome);
        Assert.Equal("Cachorro", pet.TipoAnimal);
        Assert.Equal("Labrador", pet.Raca);
        Assert.Equal("Macho", pet.Genero);
        Assert.Equal(idDono, pet.IdDono);
        Assert.NotNull(pet.Consultas);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Pet_NomeInvalido_DeveLancarDomainException(string nomeInvalido)
    {
        var act = () => new Pet(nomeInvalido, "Cachorro", "Labrador", "Macho", Guid.NewGuid());

        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Nome do pet não pode ser vazio.", ex.Message);
    }

    [Fact]
    public void Pet_IdDonoVazio_DeveLancarDomainException()
    {
        var act = () => new Pet("Rex", "Cachorro", "Labrador", "Macho", Guid.Empty);

        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Identificador do dono não pode ser vazio.", ex.Message);
    }
}
