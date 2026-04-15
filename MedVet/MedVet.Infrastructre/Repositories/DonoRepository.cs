using System;
using System.Collections.Generic;
using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IDonoRepository
{
    IReadOnlyCollection<Dono> GetAll();
    Dono? GetById(Guid id);
    void Add(Dono dono);
    
    void Update(Dono dono);
    
    void Delete(Dono dono);
    
    void SaveChanges();
    
    Dono? GetByEmail(string email);
    Dono? GetByIdWithPets(Guid id);
}