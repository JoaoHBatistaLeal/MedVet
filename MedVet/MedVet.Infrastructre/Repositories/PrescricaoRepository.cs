    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MedVet.Application.Interfaces.Repositories;
    using MedVet.Domain.Entities;
    using MedVet.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;

    namespace MedVet.Infrastructure.Repositories;

    public class PrescricaoRepository : IPrescricaoRepository
    {
        private readonly MedVetContext _context;

        public PrescricaoRepository(MedVetContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<Prescricao> GetAll()
        {
            return _context.Prescricoes.ToList();
        }

        public Prescricao? GetById(Guid id)
        {
            return _context.Prescricoes.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Prescricao prescricao)
        {
            _context.Prescricoes.Add(prescricao);
        }

        public void Update(Prescricao prescricao)
        {
            _context.Prescricoes.Update(prescricao);
        }

        public void Delete(Prescricao prescricao)
        {
            _context.Prescricoes.Remove(prescricao);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Prescricao? GetByConsultaId(Guid idConsulta)
        {
            return _context.Prescricoes
                .FirstOrDefault(p => p.IdConsulta == idConsulta);
        }
    }