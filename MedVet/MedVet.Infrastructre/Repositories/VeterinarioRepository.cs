using System;
using System.Collections.Generic;
using System.Linq;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Domain.Entities;
using MedVet.Infrastructure.Persistence;

namespace MedVet.Infrastructure.Repositories;

public class VeterinarioRepository : IVeterinarioRepository
{
    private readonly MedVetContext _context;

    public VeterinarioRepository(MedVetContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Veterinario> GetAll()
    {
        return _context.Veterinarios.ToList();
    }

    public Veterinario? GetById(Guid id)
    {
        return _context.Veterinarios.FirstOrDefault(v => v.Id == id);
    }

    public void Add(Veterinario veterinario)
    {
        _context.Veterinarios.Add(veterinario);
    }

    public void Update(Veterinario veterinario)
    {
        _context.Veterinarios.Update(veterinario);
    }

    public void Delete(Veterinario veterinario)
    {
        _context.Veterinarios.Remove(veterinario);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public Veterinario? GetByCrmv(int crmv)
    {
        return _context.Veterinarios.FirstOrDefault(v => v.Crmv == crmv);
    }
}