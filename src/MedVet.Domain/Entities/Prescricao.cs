using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Prescricao:BaseEntity
{
    public Guid IdConsulta { get; private set; }
    
    public List <Medicamento> Medicamento { get; set; }
}