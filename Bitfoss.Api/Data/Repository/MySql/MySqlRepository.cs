using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Threading;
using Bitfoss.Api.Data.Repository.StatementBuilder;
using System.Linq.Expressions;

namespace Bitfoss.Api.Data.Repository.MySql
{
    public class MySqlRepository<T> : IRepository<T> where T : new()
    {
        private readonly MySqlRepositoryOptions _options;

        private readonly IStatementBuilder _sqlStatementBuilder;

        public MySqlRepository(IOptions<MySqlRepositoryOptions> options, IStatementBuilder sqlStatementBuilder)
        {
            _options = options.Value;
            _sqlStatementBuilder = sqlStatementBuilder;
        }

        public Task<IEnumerable<T>> QueryAll(CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.Select<T>();
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            return UseConnection(c => c.QueryAsync<T>(command));
        }

        public Task<IEnumerable<T>> QueryAll(int limit, int offset = 0, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.Select<T>(limit, offset, out var limitParameter);
            var command = new CommandDefinition(sql, parameters: limitParameter, cancellationToken: cancellationToken);
            return UseConnection(c => c.QueryAsync<T>(command));
        }

        public Task<IEnumerable<T>> QueryWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.SelectWhere(expression, value, out var valueParameter);
            var command = new CommandDefinition(sql, parameters: valueParameter, cancellationToken: cancellationToken);
            return UseConnection(c => c.QueryAsync<T>(command));
        }

        public Task<IEnumerable<T>> QueryWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, int limit, int offset = 0, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.SelectWhere(expression, value, limit, offset, out var limitedValueParameter);
            var command = new CommandDefinition(sql, parameters: limitedValueParameter, cancellationToken: cancellationToken);
            return UseConnection(c => c.QueryAsync<T>(command));
        }

        public Task<T> QuerySingleOrDefaultWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.SelectWhere(expression, value, out var valueParameter);
            var command = new CommandDefinition(sql, parameters: valueParameter, cancellationToken: cancellationToken);
            return UseConnection(c => c.QuerySingleOrDefaultAsync<T>(command));
        }

        public async Task Insert(T entity, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.Insert<T>();
            var command = new CommandDefinition(sql, parameters: entity, cancellationToken: cancellationToken);
            var rowsInserted = await UseConnection(c => c.ExecuteAsync(command));

            if (rowsInserted != 1)
            {
                throw new InvalidOperationException("Failed to insert entity");
            }
        }

        public async Task Update(T entity, CancellationToken cancellationToken)
        {
            var sql = _sqlStatementBuilder.Update<T>();
            var command = new CommandDefinition(sql, parameters: entity, cancellationToken: cancellationToken);
            var rowsUpdated = await UseConnection(c => c.ExecuteAsync(command));

            if (rowsUpdated != 1)
            {
                throw new InvalidOperationException("Failed to update entity");
            }
        }

        public async Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.Delete<T>();
            var command = new CommandDefinition(sql, parameters: entity, cancellationToken: cancellationToken);
            var rowsDeleted = await UseConnection(c => c.ExecuteAsync(command));

            if (rowsDeleted != 1)
            {
                throw new InvalidOperationException("Failed to delete entity");
            }
        }

        public Task<int> DeleteWhere<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, CancellationToken cancellationToken = default)
        {
            var sql = _sqlStatementBuilder.DeleteWhere(expression, value, out var valueParameter);
            var command = new CommandDefinition(sql, parameters: valueParameter, cancellationToken: cancellationToken);
            return UseConnection(c => c.ExecuteAsync(command));
        }

        private R UseConnection<R>(Func<IDbConnection, R> func)
        {
            using var connection = new MySqlConnection(_options.ConnectionString);
            return func(connection);
        }
    }
}
