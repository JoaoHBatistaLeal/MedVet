using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Interfaces;

namespace MedVet.Application.Services.Implementations;

public sealed class VeterinarioService(IVeterinarioRepository veterinarioRepository) : IVeterinarioService
{
    public IReadOnlyList<VeterinarioResponse> GetAll()
    {
        return veterinarioRepository.GetAll().Select(VeterinarioResponse.FromDomain).ToList();
    }

    public VeterinarioResponse? GetById(Guid id)
    {
        var veterinario = veterinarioRepository.GetById(id);
        return veterinario is null ? null : VeterinarioResponse.FromDomain(veterinario);
    }

    public VeterinarioResponse Create(VeterinarioRequest request)
    {
        var veterinario = request.ToDomain();
        veterinarioRepository.Add(veterinario);
        veterinarioRepository.SaveChanges();
        return VeterinarioResponse.FromDomain(veterinario);
    }

    public bool Delete(Guid id)
    {
        var veterinario = veterinarioRepository.GetById(id);
        if (veterinario is null) return false;
        veterinarioRepository.Delete(veterinario);
        veterinarioRepository.SaveChanges();
        return true;
    }
}
