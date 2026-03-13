using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Veterinario:BaseEntity
{
    public string Nome { get; private set; }
    
    public int Crmv { get; private set; }
    
    public string Especialidade { get; private set; }
  
    public List<Consulta> Consultas { get; set; }
}