using System;
using System.Collections.Generic;
using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IConsultaRepository
{
    IReadOnlyCollection<Consulta> GetAll();
    
    Consulta? GetById(Guid id);
    
    void Add(Consulta consulta);
    
    void Update(Consulta consulta);
    
    void Delete(Consulta consulta);
    
    void SaveChanges();
    
    IReadOnlyCollection<Consulta> GetByPetId(Guid idPet);
    
    IReadOnlyCollection<Consulta> GetByVeterinarioId(Guid idVeterinario);
    
    IReadOnlyCollection<Consulta> GetByDateRange(DateTime startDate, DateTime endDate);

    Consulta? GetByIdWithPrescricao(Guid id);
    Consulta? GetByIdFull(Guid id);
    
    IReadOnlyCollection<Consulta> GetUltimasByPetId(Guid idPet, int quantidade);
}