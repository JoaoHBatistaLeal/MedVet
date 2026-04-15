using System;
using System.Collections.Generic;
using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IPrescricaoRepository
{
    IReadOnlyCollection<Prescricao> GetAll();
    Prescricao? GetById(Guid id);
    void Add(Prescricao prescricao);
    void Update(Prescricao prescricao);
    void Delete(Prescricao prescricao);
    void SaveChanges();
    
    Prescricao? GetByConsultaId(Guid idConsulta);
}