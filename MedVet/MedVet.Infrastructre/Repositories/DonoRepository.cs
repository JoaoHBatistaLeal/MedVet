using MedVet.Application.Interfaces.Repositories; // Importa a interface
using MedVet.Domain.Entities;
using MedVet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedVet.Infrastructure.Repositories;
public class DonoRepository : IDonoRepository 
{
    private readonly MedVetContext _context;

    public DonoRepository(MedVetContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Dono> GetAll()
    {
        return _context.Donos.ToList();
    }

    public Dono? GetById(Guid id)
    {
        return _context.Donos.FirstOrDefault(d => d.Id == id);
    }

    public void Add(Dono dono)
    {
        _context.Donos.Add(dono);
    }

    public void Update(Dono dono)
    {
        _context.Donos.Update(dono);
    }

    public void Delete(Dono dono)
    {
        _context.Donos.Remove(dono);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public Dono? GetByEmail(string email)
    {
        return _context.Donos.FirstOrDefault(d => d.Email == email);
    }

    public Dono? GetByIdWithPets(Guid id)
    {
        return _context.Donos
            .Include(d => d.Pets)
            .FirstOrDefault(d => d.Id == id);
    }
}