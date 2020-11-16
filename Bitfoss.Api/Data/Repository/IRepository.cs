using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bitfoss.Api.Data.Repository
{
    public interface IRepository<T> where T : new()
    {
        Task<IEnumerable<T>> QueryAll(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> QueryAll(int limit, int offset = 0, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> QueryWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> QueryWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, int limit, int offset = 0, CancellationToken cancellationToken = default);

        Task<T> QuerySingleOrDefaultWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, CancellationToken cancellationToken = default);

        Task Insert(T entity, CancellationToken cancellationToken = default);

        Task Update(T entity, CancellationToken cancellationToken = default);

        Task Delete(T entity, CancellationToken cancellationToken = default);

        Task<int> DeleteWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, CancellationToken cancellationToken = default);
    }
}
