using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Interfaces;
using MedVet.Domain.Entities;

namespace MedVet.Application.Services.Implementations;

public sealed class MedicamentoService(IRepository<Medicamento> medicamentoRepository) : IMedicamentoService
{
    public IReadOnlyList<MedicamentoResponse> GetAll()
    {
        return medicamentoRepository.GetAll().Select(MedicamentoResponse.FromDomain).ToList();
    }

    public MedicamentoResponse? GetById(Guid id)
    {
        var medicamento = medicamentoRepository.GetById(id);
        return medicamento is null ? null : MedicamentoResponse.FromDomain(medicamento);
    }

    public MedicamentoResponse Create(MedicamentoRequest request)
    {
        var medicamento = request.ToDomain();
        medicamentoRepository.Add(medicamento);
        return MedicamentoResponse.FromDomain(medicamento);
    }

    public bool Delete(Guid id)
    {
        return medicamentoRepository.Delete(id);
    }
}
