using MedVet.Application.DTOs;

namespace MedVet.Application.Services.Interfaces;

public interface IVeterinarioService
{
    IReadOnlyList<VeterinarioResponse> GetAll();
    VeterinarioResponse? GetById(Guid id);
    VeterinarioResponse Create(VeterinarioRequest request);
    bool Delete(Guid id);
}
