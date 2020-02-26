using Domain.Common.OperationHandling;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    protected readonly HubbecContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository()
    {      
      _context = new HubbecContext();
      _dbSet = _context.Set<TEntity>();
    }


    public async Task<OperationResult<TEntity>> Create(TEntity entity)
    {
      _dbSet.Add(entity);
      var result = new OperationResult<TEntity>();
      try
      {
        await _context.SaveChangesAsync();
        result.Data = entity;
        return result;
      }
      catch (Exception e)
      {        
        result.Errors.Add(e.Message);
        if (e.InnerException != null)
          result.Errors.Add(e.InnerException.Message);
        return result;
      }
    }

    public void Dispose()
    {
      _context.Dispose();
      GC.SuppressFinalize(this);
    }

    public async Task<OperationResult<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
      var result = new OperationResult<TEntity>();
      result.Data = await _dbSet.FirstOrDefaultAsync(predicate);
      if (result.Data == null) result.Errors.Add("Not found object");
      return result;
    }

    public IQueryable<TEntity> Queryable(bool @readonly = true)
    {
      return @readonly ? _dbSet.AsNoTracking().AsQueryable() : _dbSet.AsQueryable();
    }

    public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
    {
      return @readonly ? _dbSet.Where(predicate).AsNoTracking().AsQueryable() : _dbSet.Where(predicate).AsQueryable();
    }

    public async Task<OperationResult<bool>> Remove(TEntity entity)
    {      
      if (_context.Entry(entity).State == EntityState.Detached)
      {
        _dbSet.Attach(entity);
      }
      _dbSet.Remove(entity);
      var result = new OperationResult<bool>();
      try
      {
        await _context.SaveChangesAsync();
        result.Data = true;
        return result;
      }
      catch (Exception e)
      {        
        result.Errors.Add(e.Message);
        if (e.InnerException != null)
          result.Errors.Add(e.InnerException.Message);
        return result;
      }
    }

    public async Task<OperationResult<long>> Remove(Expression<Func<TEntity, bool>> predicate)
    {
      var entities = _dbSet.Where(predicate);
      var result = new OperationResult<long>();
      if (!entities.Any()) {
        result.Errors.Add("Not found objects");
        return result;
      }
      foreach (var entity in entities)
      {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
          _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
      }
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (Exception e)
      {        
        result.Errors.Add(e.Message);
        if (e.InnerException != null)
          result.Errors.Add(e.InnerException.Message);
      }

      return result;
    }

    public async Task<OperationResult<TEntity>> Update(TEntity entity)
    {
      _dbSet.Attach(entity);
      _context.Entry(entity).State = EntityState.Modified;
      var result = new OperationResult<TEntity>();
      try
      {
        await _context.SaveChangesAsync();
        result.Data = entity;
        return result;
      }
      catch (Exception e)
      {        
        result.Errors.Add(e.Message);
        if (e.InnerException != null)
          result.Errors.Add(e.InnerException.Message);
        return result;
      }
    }
  }
}
