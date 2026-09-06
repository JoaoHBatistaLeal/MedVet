using MedVet.Domain.Entities;

namespace MedVet.Application.DTOs;

public record MedicamentoRequest(string NomeMedicamento, string Marca, string ModoDeUso, double Preco)
{
    public Medicamento ToDomain() => new(NomeMedicamento, Marca, ModoDeUso, Preco);
}

public record MedicamentoResponse(Guid Id, string NomeMedicamento, string Marca, string ModoDeUso, double Preco)
{
    public static MedicamentoResponse FromDomain(Medicamento medicamento) =>
        new(medicamento.Id, medicamento.NomeMedicamento, medicamento.Marca, medicamento.ModoDeUso, medicamento.Preco);
}
