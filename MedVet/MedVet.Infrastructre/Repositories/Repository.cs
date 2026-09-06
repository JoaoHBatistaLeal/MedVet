using MedVet.Application.Interfaces.Repositories;
using MedVet.Domain.Commons;
using MedVet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedVet.Infrastructure.Repositories;

public class Repository<T>(MedVetContext context) : IRepository<T> where T : BaseEntity
{
    protected MedVetContext Context { get; } = context;
    private readonly DbSet<T> _set = context.Set<T>();

    public IReadOnlyList<T> GetAll()
    {
        return _set.AsNoTracking().ToList();
    }

    public T? GetById(Guid id)
    {
        return _set.Find(id);
    }

    public T Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _set.Add(entity);
        Context.SaveChanges();
        return entity;
    }

    public bool Delete(Guid id)
    {
        var entity = GetById(id);
        if (entity is null)
            return false;

        _set.Remove(entity);
        Context.SaveChanges();
        return true;
    }

    public bool ExistsById(Guid id)
    {
        return _set.Any(e => e.Id == id);
    }
}
