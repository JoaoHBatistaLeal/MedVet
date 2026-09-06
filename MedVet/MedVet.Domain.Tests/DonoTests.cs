using MedVet.Domain.Entities;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Tests;

public class DonoTests
{
    [Fact]
    public void Dono_DadosValidos_DeveInstanciarComSucesso()
    {
        var dono = new Dono("Carlos Silva", "carlos@email.com", "11988887777");

        Assert.Equal("Carlos Silva", dono.Nome);
        Assert.Equal("carlos@email.com", dono.Email);
        Assert.Equal("11988887777", dono.Telefone);
        Assert.NotNull(dono.Pets);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Dono_NomeInvalido_DeveLancarDomainException(string nomeInvalido)
    {
        var act = () => new Dono(nomeInvalido, "carlos@email.com", "11988887777");

        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Nome do dono não pode ser vazio.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("sem-arroba")]
    public void Dono_EmailInvalido_DeveLancarDomainException(string emailInvalido)
    {
        var act = () => new Dono("Carlos Silva", emailInvalido, "11988887777");

        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("E-mail do dono inválido.", ex.Message);
    }
}
