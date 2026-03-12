using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Pet:BaseEntity
{
    
    public Guid IdDono { get; private set; }
    
    public string Nome { get; private set; }
    
    public string TipoAnimal { get; private set; }
    
    public string Raca { get; private set; }
    
    public string Genero { get; private set; }
    
    
    public List<Consulta> Consultas { get; set; }
    
    
}