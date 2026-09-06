using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Interfaces;

namespace MedVet.Application.Services.Implementations;

public sealed class PetService(IPetRepository petRepository, IDonoRepository donoRepository) : IPetService
{
    public IReadOnlyList<PetResponse> GetAll()
    {
        return petRepository.GetAll().Select(PetResponse.FromDomain).ToList();
    }

    public PetResponse? GetById(Guid id)
    {
        var pet = petRepository.GetById(id);
        return pet is null ? null : PetResponse.FromDomain(pet);
    }

    public PetResponse Create(PetRequest request)
    {
        var dono = donoRepository.GetById(request.IdDono);
        if (dono is null)
            throw new InvalidOperationException("Dono não encontrado.");

        var pet = request.ToDomain();
        petRepository.Add(pet);
        petRepository.SaveChanges();
        return PetResponse.FromDomain(pet);
    }

    public bool Delete(Guid id)
    {
        var pet = petRepository.GetById(id);
        if (pet is null) return false;
        petRepository.Delete(pet);
        petRepository.SaveChanges();
        return true;
    }
}
