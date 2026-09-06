using MedVet.Domain.Commons;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Entities;

public class Consulta : BaseEntity
{
    public Guid IdPet { get; private set; }
    public Guid IdVeterinario { get; private set; }
    public DateTime DataConsulta { get; private set; }
    public string Diagnostico { get; private set; }
    public string Observacoes { get; private set; }
    public Prescricao Prescricoes { get; set; }
    public Veterinario Veterinario { get; set; }
    public Pet Pet { get; private set; }

    protected Consulta()
    {
        Diagnostico = string.Empty;
        Observacoes = string.Empty;
    }

    public Consulta(Guid idPet, Guid idVeterinario, DateTime dataConsulta, string diagnostico, string observacoes)
    {
        if (idPet == Guid.Empty)
            throw new DomainException("Identificador do pet não pode ser vazio.");
        if (idVeterinario == Guid.Empty)
            throw new DomainException("Identificador do veterinário não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(diagnostico))
            throw new DomainException("Diagnóstico não pode ser vazio.");

        IdPet = idPet;
        IdVeterinario = idVeterinario;
        DataConsulta = dataConsulta;
        Diagnostico = diagnostico;
        Observacoes = observacoes ?? string.Empty;
    }
}