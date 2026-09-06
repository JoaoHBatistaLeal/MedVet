using MedVet.Domain.Commons;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Entities;

public class Veterinario : BaseEntity
{
    public string Nome { get; private set; }
    public int Crmv { get; private set; }
    public string Especialidade { get; private set; }
    public List<Consulta> Consultas { get; set; }

    protected Veterinario()
    {
        Nome = string.Empty;
        Especialidade = string.Empty;
        Consultas = new List<Consulta>();
    }

    public Veterinario(string nome, int crmv, string especialidade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do veterinário não pode ser vazio.");
        if (crmv <= 0)
            throw new DomainException("CRMV deve ser um número positivo.");

        Nome = nome;
        Crmv = crmv;
        Especialidade = especialidade;
        Consultas = new List<Consulta>();
    }
}