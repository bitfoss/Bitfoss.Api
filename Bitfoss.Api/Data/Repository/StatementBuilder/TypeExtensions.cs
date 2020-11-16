using Bitfoss.Api.Data.Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bitfoss.Api.Data.Repository.StatementBuilder
{
    public static class TypeExtensions
    {
        public static EntityDescription GetDataEntityDescription(this Type type)
        {
            var dataEntityDescription = new EntityDescription
            {
                EntityName = type.GetEntityName(),
                PropertyDescriptions = type.GetDataEntityPropertyDescriptions()
            };

            return dataEntityDescription;
        }

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Type type, Expression<Func<TSource, TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property");
            }

            var propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property");
            }

            if (type != propertyInfo.ReflectedType && !type.IsSubclassOf(propertyInfo.ReflectedType))
            {
                throw new ArgumentException($"Expression '{expression}' refers to a property that is not from type {type}");
            }

            return propertyInfo;
        }

        private static IEnumerable<EntityFieldDescription> GetDataEntityPropertyDescriptions(this Type type)
        {
            return type
                .GetProperties()
                .Where(property => property.GetCustomAttribute<EntityField>() != default)
                .Select(property => new EntityFieldDescription
                {
                    EntityFieldName = property.GetCustomAttribute<EntityField>()?.Name ?? property.Name,
                    IsPrimaryKey = property.GetCustomAttribute<EntityField>().IsPrimaryKey,
                    PropertyName = property.Name
                });
        }

        private static string GetEntityName(this Type type)
        {
            var attribute = type.GetCustomAttribute<Entity>() ?? throw new InvalidOperationException($"{type.Name} does not have the {nameof(Entity)} attribute");
            return attribute.Name ?? type.Name;
        }
    }
}
