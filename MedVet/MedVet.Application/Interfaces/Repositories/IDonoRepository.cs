using MedVet.Domain.Entities;

namespace MedVet.Application.Interfaces.Repositories;

public interface IDonoRepository
{
    IReadOnlyCollection<Dono> GetAll();
    
    /// <summary>
    /// Obtém um dono pelo ID
    /// </summary>
    Dono? GetById(int id);
    
    /// <summary>
    /// Adiciona um novo dono
    /// </summary>
    void Add(Dono dono);
    
    /// <summary>
    /// Atualiza um dono existente
    /// </summary>
    void Update(Dono dono);
    
    /// <summary>
    /// Remove um dono
    /// </summary>
    void Delete(Dono dono);
    
    /// <summary>
    /// Salva as alterações no banco de dados
    /// </summary>
    void SaveChanges();
    
    // Métodos específicos adicionais
    /// <summary>
    /// Obtém um dono pelo email
    /// </summary>
    Dono? GetByEmail(string email);
    
    /// <summary>
    /// Obtém um dono com seus pets
    /// </summary>
    Dono? GetByIdWithPets(int id);
}