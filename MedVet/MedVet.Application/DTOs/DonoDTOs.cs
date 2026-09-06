using MedVet.Domain.Entities;

namespace MedVet.Application.DTOs;

public record DonoRequest(string Nome, string Email, string Telefone)
{
    public Dono ToDomain() => new(Nome, Email, Telefone);
}

public record DonoResponse(Guid Id, string Nome, string Email, string Telefone)
{
    public static DonoResponse FromDomain(Dono dono) =>
        new(dono.Id, dono.Nome, dono.Email, dono.Telefone);
}
