using Microsoft.EntityFrameworkCore;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Domain.Entities;
using MedVet.Infrastructure.Persistence;

namespace MedVet.Infrastructure.Repositories;

/// <summary>
/// Repositório EF de Consulta.
/// Centraliza acesso a dados da entidade <see cref="Consulta"/>.
/// </summary>
public class ConsultaRepository(MedVetContext context) : IConsultaRepository
{
    /// <summary>
    /// Retorna todas as consultas com seus Pets e Veterinários.
    /// </summary>
    public IReadOnlyCollection<Consulta> GetAll()
    {
        return context.Consultas
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .ToList();
    }

    /// <summary>
    /// Busca consulta por Id com carregamento de Pet e Veterinário.
    /// </summary>
    public Consulta? GetById(int id)
    {
        return context.Consultas
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Adiciona uma nova consulta ao contexto.
    /// </summary>
    public void Add(Consulta consulta)
    {
        context.Consultas.Add(consulta);
    }

    /// <summary>
    /// Marca consulta como alterada no contexto.
    /// </summary>
    public void Update(Consulta consulta)
    {
        context.Consultas.Update(consulta);
    }

    /// <summary>
    /// Remove consulta do contexto.
    /// </summary>
    public void Delete(Consulta consulta)
    {
        context.Consultas.Remove(consulta);
    }

    /// <summary>
    /// Persiste alterações pendentes no banco.
    /// </summary>
    public void SaveChanges()
    {
        context.SaveChanges();
    }

    /// <summary>
    /// Busca consultas por Pet.
    /// </summary>
    public IReadOnlyCollection<Consulta> GetByPetId(Guid idPet)
    {
        return context.Consultas
            .Where(x => x.IdPet == idPet)
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .OrderByDescending(x => x.DataConsulta)
            .ToList();
    }

    /// <summary>
    /// Busca consultas por Veterinário.
    /// </summary>
    public IReadOnlyCollection<Consulta> GetByVeterinarioId(Guid idVeterinario)
    {
        return context.Consultas
            .Where(x => x.IdVeterinario == idVeterinario)
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .OrderByDescending(x => x.DataConsulta)
            .ToList();
    }

    /// <summary>
    /// Busca consultas por período de data.
    /// </summary>
    public IReadOnlyCollection<Consulta> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return context.Consultas
            .Where(x => x.DataConsulta.Date >= startDate.Date && 
                        x.DataConsulta.Date <= endDate.Date)
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .OrderBy(x => x.DataConsulta)
            .ToList();
    }

    /// <summary>
    /// Busca consulta por Id com carregamento da Prescrição.
    /// </summary>
    public Consulta? GetByIdWithPrescricao(int id)
    {
        return context.Consultas
            .Include(x => x.Prescricoes)
            .FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Busca consulta completa (Pet, Veterinário e Prescrição com Medicamentos).
    /// </summary>
    public Consulta? GetByIdFull(int id)
    {
        return context.Consultas
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .Include(x => x.Prescricoes)
                .ThenInclude(p => p.Medicamento)
            .FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Busca as últimas N consultas de um Pet.
    /// </summary>
    public IReadOnlyCollection<Consulta> GetUltimasByPetId(Guid idPet, int quantidade)
    {
        return context.Consultas
            .Where(x => x.IdPet == idPet)
            .Include(x => x.Pet)
            .Include(x => x.Veterinario)
            .OrderByDescending(x => x.DataConsulta)
            .Take(quantidade)
            .ToList();
    }
}