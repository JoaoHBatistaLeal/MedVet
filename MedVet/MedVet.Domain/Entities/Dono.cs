using MedVet.Domain.Commons;

namespace MedVet.Domain.Entities;

public class Dono:BaseEntity
{
    public string Nome { get; private set; }
    
    public string Email { get; private set; }
    
    public string Telefone { get; private set; }
    
    public List<Pet> Pets { get; set; }
    
    protected Dono() { }

    public Dono(string nome, string email, string telefone)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Pets = new List<Pet>();
    }
}