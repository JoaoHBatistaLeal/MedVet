using MedVet.Application.DTOs;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Interfaces;

namespace MedVet.Application.Services.Implementations;

public sealed class ConsultaService(
    IConsultaRepository consultaRepository,
    IPetRepository petRepository,
    IVeterinarioRepository veterinarioRepository) : IConsultaService
{
    public IReadOnlyList<ConsultaResponse> GetAll()
    {
        return consultaRepository.GetAll().Select(ConsultaResponse.FromDomain).ToList();
    }

    public ConsultaResponse? GetById(Guid id)
    {
        var consulta = consultaRepository.GetById(id);
        return consulta is null ? null : ConsultaResponse.FromDomain(consulta);
    }

    public ConsultaResponse Create(ConsultaRequest request)
    {
        var pet = petRepository.GetById(request.IdPet);
        if (pet is null)
            throw new InvalidOperationException("Pet não encontrado.");

        var vet = veterinarioRepository.GetById(request.IdVeterinario);
        if (vet is null)
            throw new InvalidOperationException("Veterinário não encontrado.");

        var consulta = request.ToDomain();
        consultaRepository.Add(consulta);
        consultaRepository.SaveChanges();
        return ConsultaResponse.FromDomain(consulta);
    }

    public bool Delete(Guid id)
    {
        var consulta = consultaRepository.GetById(id);
        if (consulta is null) return false;
        consultaRepository.Delete(consulta);
        consultaRepository.SaveChanges();
        return true;
    }
}
