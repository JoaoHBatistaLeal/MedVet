using MedVet.Domain.Entities;

namespace MedVet.Application.DTOs;

public record VeterinarioRequest(string Nome, int Crmv, string Especialidade)
{
    public Veterinario ToDomain() => new(Nome, Crmv, Especialidade);
}

public record VeterinarioResponse(Guid Id, string Nome, int Crmv, string Especialidade)
{
    public static VeterinarioResponse FromDomain(Veterinario veterinario) =>
        new(veterinario.Id, veterinario.Nome, veterinario.Crmv, veterinario.Especialidade);
}
