using MedVet.Domain.Commons;

namespace MedVet.Application.Interfaces.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    IReadOnlyList<T> GetAll();
    (IReadOnlyList<T> Items, int Total) GetPaged(int pageNumber, int pageSize);
    T? GetById(Guid id);
    T Add(T entity);
    bool Delete(Guid id);
    bool ExistsById(Guid id);
}
