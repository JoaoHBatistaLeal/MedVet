using MedVet.Domain.Entities;

namespace MedVet.Application.DTOs;

public record PetRequest(string Nome, string TipoAnimal, string Raca, string Genero, Guid IdDono)
{
    public Pet ToDomain() => new(Nome, TipoAnimal, Raca, Genero, IdDono);
}

public record PetResponse(Guid Id, string Nome, string TipoAnimal, string Raca, string Genero, Guid IdDono)
{
    public static PetResponse FromDomain(Pet pet) =>
        new(pet.Id, pet.Nome, pet.TipoAnimal, pet.Raca, pet.Genero, pet.IdDono);
}
