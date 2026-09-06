using MedVet.Domain.Entities;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Tests;

public class MedicamentoTests
{
    [Fact]
    public void Medicamento_DadosValidos_DeveInstanciarComSucesso()
    {
        var medicamento = new Medicamento("Amoxicilina", "VetPharma", "Oral a cada 12 horas", 45.90);

        Assert.Equal("Amoxicilina", medicamento.NomeMedicamento);
        Assert.Equal("VetPharma", medicamento.Marca);
        Assert.Equal("Oral a cada 12 horas", medicamento.ModoDeUso);
        Assert.Equal(45.90, medicamento.Preco);
        Assert.NotNull(medicamento.Prescricoes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-25.50)]
    public void Medicamento_PrecoInvalido_DeveLancarDomainException(double precoInvalido)
    {
        var act = () => new Medicamento("Dipirona", "VetLab", "Gotas", precoInvalido);

        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Preço deve ser maior que zero.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Medicamento_NomeInvalido_DeveLancarDomainException(string nomeInvalido)
    {
        var act = () => new Medicamento(nomeInvalido, "VetLab", "Gotas", 20.0);

        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Nome do medicamento não pode ser vazio.", ex.Message);
    }
}
