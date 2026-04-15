using System;
using System.Collections.Generic;
using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IMedicamentoRepository
{
    IReadOnlyCollection<Medicamento> GetAll();
    Medicamento? GetById(Guid id);
    void Add(Medicamento medicamento);
    void Update(Medicamento medicamento);
    void Delete(Medicamento medicamento);
    void SaveChanges();
    
    IReadOnlyCollection<Medicamento> GetByNome(string nome);
}