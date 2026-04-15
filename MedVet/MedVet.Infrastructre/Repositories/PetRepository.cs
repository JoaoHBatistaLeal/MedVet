using System;
using System.Collections.Generic;
using System.Linq;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Domain.Entities;
using MedVet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedVet.Infrastructure.Repositories;

public class PetRepository : IPetRepository
{
    private readonly MedVetContext _context;

    public PetRepository(MedVetContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Pet> GetAll()
    {
        return _context.Pets.ToList();
    }

    public Pet? GetById(Guid id)
    {
        return _context.Pets.FirstOrDefault(p => p.Id == id);
    }

    public void Add(Pet pet)
    {
        _context.Pets.Add(pet);
    }

    public void Update(Pet pet)
    {
        _context.Pets.Update(pet);
    }

    public void Delete(Pet pet)
    {
        _context.Pets.Remove(pet);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public IReadOnlyCollection<Pet> GetByIdDono(Guid idDono)
    {
        return _context.Pets.Where(p => p.IdDono == idDono).ToList();
    }

    public Pet? GetByIdWithConsultas(Guid id)
    {
        return _context.Pets
            .Include(p => p.Consultas)
            .FirstOrDefault(p => p.Id == id);
    }
}