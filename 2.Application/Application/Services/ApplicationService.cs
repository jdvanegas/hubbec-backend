using Application.Interfaces;
using Domain.Common.OperationHandling;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
  public class ApplicationService<TEntity> : IApplicationService<TEntity> where TEntity : class
  {
    private readonly IRepository<TEntity> _repository;

    public ApplicationService(IRepository<TEntity> repository)
    {
      _repository = repository;
    }

    public async Task<OperationResult<TEntity>> Create(TEntity entity)
    {
      return await _repository.Create(entity);
    }

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }

    public async Task<OperationResult<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
      return await _repository.Get(predicate);
    }

    public IQueryable<TEntity> Queryable(bool @readonly = true)
    {
      return _repository.Queryable(@readonly);
    }

    public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
    {
      return _repository.Queryable(predicate, @readonly);
    }

    public async Task<OperationResult<bool>> Remove(TEntity entity)
    {
      return await _repository.Remove(entity);
    }

    public async Task<OperationResult<long>> Remove(Expression<Func<TEntity, bool>> predicate)
    {
      return await _repository.Remove(predicate);
    }

    public async Task<OperationResult<TEntity>> Update(TEntity entity)
    {
      return await _repository.Update(entity);
    }
  }
}
