using Bitfoss.Api.Data.Repository.Attributes;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bitfoss.Api.Data.Repository.StatementBuilder
{
    public class StatementBuilder : IStatementBuilder
    {
        public string Select<T>()
        {
            var description = typeof(T).GetDataEntityDescription().Validate();
            var columnNames = string.Join(", ", description.PropertyDescriptions.Select(ColumnNameAsPropertyName));
            return $"SELECT {columnNames} FROM {description.EntityName}";
        }

        public string Select<T>(int limit, int offset, out LimitParameter limitParameter)
        {
            limitParameter = new LimitParameter { Limit = limit, Offset = offset };
            return $"{Select<T>()} LIMIT @{nameof(limitParameter.Limit)} OFFSET @{nameof(limitParameter.Offset)}";
        }

        public string SelectWhere<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, out ValueParameter<TProperty> parameter)
        {
            var description = typeof(T).GetDataEntityDescription().Validate();
            var columnNames = string.Join(", ", description.PropertyDescriptions.Select(ColumnNameAsPropertyName));
            var columnName = GetColumnNameFromExpression(expression);
            parameter = new ValueParameter<TProperty> { Value = value };
            return $"SELECT {columnNames} FROM {description.EntityName} WHERE {columnName} = @{nameof(parameter.Value)}";
        }

        public string SelectWhere<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, int limit, int offset, out LimitedValueParameter<TProperty> parameter)
        {
            var sql = SelectWhere(expression, value, out var _);
            parameter = new LimitedValueParameter<TProperty> { Value = value, Limit = limit, Offset = offset };
            return $"{sql} LIMIT @{nameof(parameter.Limit)} OFFSET @{nameof(parameter.Offset)}";
        }

        public string Insert<T>()
        {
            var description = typeof(T).GetDataEntityDescription().Validate();
            var columnNames = string.Join(", ", description.PropertyDescriptions.Select(c => c.EntityFieldName));
            var columnValues = string.Join(", ", description.PropertyDescriptions.Select(c => "@" + c.PropertyName));
            return $"INSERT INTO {description.EntityName} ({columnNames}) VALUES ({columnValues})";
        }

        public string Update<T>()
        {
            var description = typeof(T).GetDataEntityDescription().Validate();
            var primaryKey = description.PropertyDescriptions.Single(c => c.IsPrimaryKey);
            var updates = string.Join(", ", description.PropertyDescriptions.Where(c => !c.IsPrimaryKey).Select(c => $"{c.EntityFieldName} = @{c.PropertyName}"));
            return $"UPDATE {description.EntityName} SET {updates} WHERE {primaryKey.EntityFieldName} = @{primaryKey.PropertyName}";
        }

        public string Delete<T>()
        {
            var description = typeof(T).GetDataEntityDescription().Validate();
            var primaryProperty = description.PropertyDescriptions.Single(c => c.IsPrimaryKey);
            return $"DELETE FROM {description.EntityName} WHERE {primaryProperty.EntityFieldName} = @{primaryProperty.PropertyName}";
        }

        public string DeleteWhere<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value, out ValueParameter<TProperty> parameter)
        {
            var description = typeof(T).GetDataEntityDescription().Validate();
            var columnName = GetColumnNameFromExpression(expression);
            parameter = new ValueParameter<TProperty> { Value = value };
            return $"DELETE FROM {description.EntityName} WHERE {columnName} = @{nameof(parameter.Value)}";
        }

        private string ColumnNameAsPropertyName(EntityFieldDescription propertyDescription)
        {
            if (propertyDescription.EntityFieldName == propertyDescription.PropertyName)
            {
                return propertyDescription.EntityFieldName;
            }
            else
            {
                return $"{propertyDescription.EntityFieldName} AS {propertyDescription.PropertyName}";
            }
        }

        private string GetColumnNameFromExpression<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propertyInfo = typeof(T).GetPropertyInfo(expression);
            var propertyName = propertyInfo.Name;
            return propertyInfo.GetCustomAttribute<EntityField>().Name ?? propertyName;
        }
    }
}
