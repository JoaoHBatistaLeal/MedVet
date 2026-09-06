using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Interfaces;

namespace MedVet.Application.Services.Implementations;

public sealed class DonoService(IDonoRepository donoRepository) : IDonoService
{
    public IReadOnlyList<DonoResponse> GetAll()
    {
        return donoRepository.GetAll().Select(DonoResponse.FromDomain).ToList();
    }

    public DonoResponse? GetById(Guid id)
    {
        var dono = donoRepository.GetById(id);
        return dono is null ? null : DonoResponse.FromDomain(dono);
    }

    public DonoResponse Create(DonoRequest request)
    {
        var dono = request.ToDomain();
        donoRepository.Add(dono);
        donoRepository.SaveChanges();
        return DonoResponse.FromDomain(dono);
    }

    public bool Delete(Guid id)
    {
        var dono = donoRepository.GetById(id);
        if (dono is null) return false;
        donoRepository.Delete(dono);
        donoRepository.SaveChanges();
        return true;
    }
}
