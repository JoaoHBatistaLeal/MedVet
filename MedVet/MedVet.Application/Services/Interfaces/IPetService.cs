using MedVet.Application.DTOs;

namespace MedVet.Application.Services.Interfaces;

public interface IPetService
{
    IReadOnlyList<PetResponse> GetAll();
    PetResponse? GetById(Guid id);
    PetResponse Create(PetRequest request);
    bool Delete(Guid id);
}
