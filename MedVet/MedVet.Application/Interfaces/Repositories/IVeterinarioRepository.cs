using System;
using System.Collections.Generic;
using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IVeterinarioRepository
{
    IReadOnlyCollection<Veterinario> GetAll();
    Veterinario? GetById(Guid id);
    void Add(Veterinario veterinario);
    void Update(Veterinario veterinario);
    void Delete(Veterinario veterinario);
    void SaveChanges();

    Veterinario? GetByCrmv(int crmv); 
}