using System;
using System.Collections.Generic;
using System.Linq;
using MedVet.Application.Interfaces.Repositories;
using MedVet.Domain.Entities;
using MedVet.Infrastructure.Persistence;

namespace MedVet.Infrastructure.Repositories;

public class MedicamentoRepository : IMedicamentoRepository
{
    private readonly MedVetContext _context;

    public MedicamentoRepository(MedVetContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Medicamento> GetAll()
    {
        return _context.Medicamentos.ToList();
    }

    public Medicamento? GetById(Guid id)
    {
        return _context.Medicamentos.FirstOrDefault(m => m.Id == id);
    }

    public void Add(Medicamento medicamento)
    {
        _context.Medicamentos.Add(medicamento);
    }

    public void Update(Medicamento medicamento)
    {
        _context.Medicamentos.Update(medicamento);
    }

    public void Delete(Medicamento medicamento)
    {
        _context.Medicamentos.Remove(medicamento);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public IReadOnlyCollection<Medicamento> GetByNome(string nome)
    {
        return _context.Medicamentos
            .Where(m => m.NomeMedicamento.Contains(nome))
            .ToList();
    }
}