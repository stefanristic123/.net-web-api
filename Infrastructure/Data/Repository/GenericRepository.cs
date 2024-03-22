using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context){
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id){
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(){
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecifications<T> spec){
            return await ApplaySpecifications(spec).FirstOrDefaultAsync();
        } 

        public async Task<IReadOnlyList<T>> ListAsync(ISpecifications<T> spec){
            return await ApplaySpecifications(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecifications<T> spec)
        {
            return await ApplaySpecifications(spec).CountAsync();
        }

        private IQueryable<T> ApplaySpecifications(ISpecifications<T> spec){
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}