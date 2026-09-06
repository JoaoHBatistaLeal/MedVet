using MedVet.Application.DTOs;

namespace MedVet.Application.Services.Interfaces;

public interface IConsultaService
{
    IReadOnlyList<ConsultaResponse> GetAll();
    ConsultaResponse? GetById(Guid id);
    ConsultaResponse Create(ConsultaRequest request);
    bool Delete(Guid id);
}
