using MedVet.Domain.Commons;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Entities;

public class Dono : BaseEntity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }
    public List<Pet> Pets { get; set; }

    protected Dono()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        Pets = new List<Pet>();
    }

    public Dono(string nome, string email, string telefone)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do dono não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("E-mail do dono inválido.");

        Nome = nome;
        Email = email;
        Telefone = telefone;
        Pets = new List<Pet>();
    }
}