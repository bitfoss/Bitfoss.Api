using System;
using System.Linq.Expressions;

namespace Bitfoss.Api.Data.Repository.StatementBuilder
{
    public interface IStatementBuilder
    {
        string Select<T>();

        string Select<T>(int limit, int offset, out LimitParameter limitParameter);

        string SelectWhere<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, out ValueParameter<TProperty> parameter);

        string SelectWhere<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, int limit, int offset, out LimitedValueParameter<TProperty> parameter);

        string Insert<T>();

        string Update<T>();

        string Delete<T>();

        string DeleteWhere<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, out ValueParameter<TProperty> parameter);
    }
}
