using Domain.Common.OperationHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
  public interface IApplicationService<TEntity> : IDisposable where TEntity : class
  {
    Task<OperationResult<TEntity>> Create(TEntity entity);
    Task<OperationResult<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> Queryable(bool @readonly = true);
    IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true);
    Task<OperationResult<bool>> Remove(TEntity entity);
    Task<OperationResult<long>> Remove(Expression<Func<TEntity, bool>> predicate);
    Task<OperationResult<TEntity>> Update(TEntity entity);
  }
}