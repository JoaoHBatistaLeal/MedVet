using MedVet.Application.Interfaces.Repositories;
using MedVet.Domain.Entities;
using MedVet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedVet.Infrastructure.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly MedVetContext _context;

    public ConsultaRepository(MedVetContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Consulta> GetAll()
    {
        return _context.Consultas.ToList();
    }
    
    public Consulta? GetById(Guid id)
    {
        return _context.Consultas.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Consulta consulta)
    {
        _context.Consultas.Add(consulta);
    }

    public void Update(Consulta consulta)
    {
        _context.Consultas.Update(consulta);
    }

    public void Delete(Consulta consulta)
    {
        _context.Consultas.Remove(consulta);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public IReadOnlyCollection<Consulta> GetByPetId(Guid idPet)
    {
        return _context.Consultas
            .Where(c => c.IdPet == idPet)
            .ToList();
    }

    public IReadOnlyCollection<Consulta> GetByVeterinarioId(Guid idVeterinario)
    {
        return _context.Consultas
            .Where(c => c.IdVeterinario == idVeterinario)
            .ToList();
    }

    public IReadOnlyCollection<Consulta> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.Consultas
            .Where(c => c.DataConsulta >= startDate && c.DataConsulta <= endDate)
            .ToList();
    }
    public Consulta? GetByIdWithPrescricao(Guid id)
    {
        return _context.Consultas
            .Include(c => c.Prescricoes) 
            .FirstOrDefault(c => c.Id == id);
    }

    public Consulta? GetByIdFull(Guid id)
    {
        return _context.Consultas
            .Include(c => c.Pet)
            .Include(c => c.Veterinario)
            .Include(c => c.Prescricoes)
            .FirstOrDefault(c => c.Id == id);
    }

    public IReadOnlyCollection<Consulta> GetUltimasByPetId(Guid idPet, int quantidade)
    {
        return _context.Consultas
            .Where(c => c.IdPet == idPet)
            .OrderByDescending(c => c.DataConsulta)
            .Take(quantidade)
            .ToList();
    }
}