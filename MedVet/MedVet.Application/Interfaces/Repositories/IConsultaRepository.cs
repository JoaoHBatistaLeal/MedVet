using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IConsultaRepository
{
    /// <summary>
    /// Obtém todas as consultas
    /// </summary>
    IReadOnlyCollection<Consulta> GetAll();
    
    /// <summary>
    /// Obtém uma consulta pelo ID
    /// </summary>
    Consulta? GetById(int id);
    
    /// <summary>
    /// Adiciona uma nova consulta
    /// </summary>
    void Add(Consulta consulta);
    
    /// <summary>
    /// Atualiza uma consulta existente
    /// </summary>
    void Update(Consulta consulta);
    
    /// <summary>
    /// Remove uma consulta
    /// </summary>
    void Delete(Consulta consulta);
    
    /// <summary>
    /// Salva as alterações no banco de dados
    /// </summary>
    void SaveChanges();
    
    // Métodos específicos da Consulta
    
    /// <summary>
    /// Obtém consultas por Pet
    /// </summary>
    IReadOnlyCollection<Consulta> GetByPetId(Guid idPet);
    
    /// <summary>
    /// Obtém consultas por Veterinário
    /// </summary>
    IReadOnlyCollection<Consulta> GetByVeterinarioId(Guid idVeterinario);
    
    /// <summary>
    /// Obtém consultas por período
    /// </summary>
    IReadOnlyCollection<Consulta> GetByDateRange(DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Obtém consulta com prescrição
    /// </summary>
    Consulta? GetByIdWithPrescricao(int id);
    
    /// <summary>
    /// Obtém consulta completa (com Pet, Veterinário e Prescrição)
    /// </summary>
    Consulta? GetByIdFull(int id);
    
    /// <summary>
    /// Obtém últimas consultas de um Pet
    /// </summary>
    IReadOnlyCollection<Consulta> GetUltimasByPetId(Guid idPet, int quantidade);
}