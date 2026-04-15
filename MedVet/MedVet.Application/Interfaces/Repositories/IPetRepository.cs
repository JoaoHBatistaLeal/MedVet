using System;
using System.Collections.Generic;
using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IPetRepository
{
    IReadOnlyCollection<Pet> GetAll();
    Pet? GetById(Guid id);
    void Add(Pet pet);
    void Update(Pet pet);
    void Delete(Pet pet);
    void SaveChanges();
    
    IReadOnlyCollection<Pet> GetByIdDono(Guid idDono);
    Pet? GetByIdWithConsultas(Guid id);
}