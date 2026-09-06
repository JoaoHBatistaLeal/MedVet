using MedVet.Domain.Entities;

namespace MedVet.Application.DTOs;

public record ConsultaRequest(Guid IdPet, Guid IdVeterinario, DateTime DataConsulta, string Diagnostico, string Observacoes)
{
    public Consulta ToDomain() => new(IdPet, IdVeterinario, DataConsulta, Diagnostico, Observacoes);
}

public record ConsultaResponse(Guid Id, Guid IdPet, Guid IdVeterinario, DateTime DataConsulta, string Diagnostico, string Observacoes)
{
    public static ConsultaResponse FromDomain(Consulta consulta) =>
        new(consulta.Id, consulta.IdPet, consulta.IdVeterinario, consulta.DataConsulta, consulta.Diagnostico, consulta.Observacoes);
}
