using MedVet.Application.DTOs;

namespace MedVet.Application.Services.Interfaces;

public interface IMedicamentoService
{
    IReadOnlyList<MedicamentoResponse> GetAll();
    (IReadOnlyList<MedicamentoResponse> Items, int Total) GetPaged(int pageNumber, int pageSize);
    MedicamentoResponse? GetById(Guid id);
    MedicamentoResponse Create(MedicamentoRequest request);
    bool Delete(Guid id);
}
