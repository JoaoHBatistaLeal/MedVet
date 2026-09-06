using MedVet.Application.DTOs;

namespace MedVet.Application.Services.Interfaces;

public interface IMedicamentoService
{
    IReadOnlyList<MedicamentoResponse> GetAll();
    MedicamentoResponse? GetById(Guid id);
    MedicamentoResponse Create(MedicamentoRequest request);
    bool Delete(Guid id);
}
