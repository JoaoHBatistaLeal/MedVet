using MedVet.Application.DTOs;

namespace MedVet.Application.Services.Interfaces;

public interface IDonoService
{
    IReadOnlyList<DonoResponse> GetAll();
    DonoResponse? GetById(Guid id);
    DonoResponse Create(DonoRequest request);
    bool Delete(Guid id);
}
