using System;
using Dapper;
using Bitfoss.Api.Data.Repository.MySql;
using Bitfoss.Api.Data.Repository.StatementBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace Bitfoss.Api.Data.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMySqlRepository(this IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            services.AddTransient<IStatementBuilder, StatementBuilder.StatementBuilder>();
            services.AddTransient(typeof(IRepository<>), typeof(MySqlRepository<>));
        }
    }
}
