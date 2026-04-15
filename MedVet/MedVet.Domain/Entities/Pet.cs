
using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Pet : BaseEntity
{
    public Guid IdDono { get; private set; }
    public string Nome { get; private set; }
    public string TipoAnimal { get; private set; }
    public string Raca { get; private set; }
    public string Genero { get; private set; }
    public Dono Dono { get; set; }
    
    public virtual List<Consulta> Consultas { get; set; }
    protected Pet() { }

    public Pet(string nome, string tipoAnimal, string raca, string genero, Guid idDono)
    {
        Nome = nome;
        TipoAnimal = tipoAnimal;
        Raca = raca;
        Genero = genero;
        IdDono = idDono;
        Consultas = new List<Consulta>();
    }
}