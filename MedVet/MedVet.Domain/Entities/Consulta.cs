using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Consulta : BaseEntity
{
    public Guid IdPet { get; private set; }

    public Guid IdVeterinario { get; private set; }

    public DateTime DataConsulta { get; private set; }

    public string Diagnostico { get; private set; }
    
    public string Observacoes { get; private set; }
    
    public Prescricao Prescricoes { get; set;} 
    
    public Veterinario Veterinario { get; set;}
    
    public Pet Pet { get; private set; }
}