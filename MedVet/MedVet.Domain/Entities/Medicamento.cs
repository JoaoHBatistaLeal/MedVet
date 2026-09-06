using MedVet.Domain.Commons;
using MedVet.Domain.Exceptions;

namespace MedVet.Domain.Entities;

public class Medicamento : BaseEntity
{
    public string NomeMedicamento { get; private set; }
    public string Marca { get; private set; }
    public string ModoDeUso { get; private set; }
    public double Preco { get; private set; }
    public List<Prescricao> Prescricoes { get; set; }

    protected Medicamento()
    {
        NomeMedicamento = string.Empty;
        Marca = string.Empty;
        ModoDeUso = string.Empty;
        Prescricoes = new List<Prescricao>();
    }

    public Medicamento(string nomeMedicamento, string marca, string modoDeUso, double preco)
    {
        if (string.IsNullOrWhiteSpace(nomeMedicamento))
            throw new DomainException("Nome do medicamento não pode ser vazio.");
        if (preco <= 0)
            throw new DomainException("Preço deve ser maior que zero.");

        NomeMedicamento = nomeMedicamento;
        Marca = marca;
        ModoDeUso = modoDeUso;
        Preco = preco;
        Prescricoes = new List<Prescricao>();
    }
}