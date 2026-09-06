using MedVet.Domain.Commons;
using MedVet.Domain.Exceptions;

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

    protected Pet()
    {
        Nome = string.Empty;
        TipoAnimal = string.Empty;
        Raca = string.Empty;
        Genero = string.Empty;
        Consultas = new List<Consulta>();
    }

    public Pet(string nome, string tipoAnimal, string raca, string genero, Guid idDono)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do pet não pode ser vazio.");
        if (idDono == Guid.Empty)
            throw new DomainException("Identificador do dono não pode ser vazio.");

        Nome = nome;
        TipoAnimal = tipoAnimal;
        Raca = raca;
        Genero = genero;
        IdDono = idDono;
        Consultas = new List<Consulta>();
    }
}