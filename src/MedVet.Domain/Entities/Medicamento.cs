using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Medicamento:BaseEntity
{
    public string NomeMedicamento { get; private set; }
    
    public string Marca { get; private set; }
    
    public string ModoDeUso { get; private set; }
    
    public double Preco { get; private set; }
    
    
    public List<Prescricao> Prescricoes { get; set; }
}