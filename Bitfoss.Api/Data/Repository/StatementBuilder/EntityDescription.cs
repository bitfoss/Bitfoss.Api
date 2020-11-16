using Bitfoss.Api.Data.Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitfoss.Api.Data.Repository.StatementBuilder
{
    public class EntityDescription
    {
        private static readonly char[] _validSpecialCharacters = new[] { '_' };

        public string EntityName { get; set; }

        public IEnumerable<EntityFieldDescription> PropertyDescriptions { get; set; }

        public EntityDescription Validate()
        {
            if (!IsLettersOnly(EntityName))
            {
                throw new InvalidOperationException($"Invalid {nameof(Entity)} name: '{EntityName}'");
            }

            if (!HasOnePrimaryKey())
            {
                throw new InvalidOperationException($"{nameof(Entity)} must have 1 primary key");
            }

            var invalidProperty = PropertyDescriptions.FirstOrDefault(property => !IsLettersOnly(property.EntityFieldName));
            if (invalidProperty is EntityFieldDescription property)
            {
                throw new InvalidOperationException($"Invalid {nameof(EntityField)} name: '{property.EntityFieldName}'");
            }

            foreach (var propertyDescription in PropertyDescriptions)
            {
                if (PropertyDescriptions.Count(p => p.EntityFieldName == propertyDescription.EntityFieldName) != 1)
                {
                    throw new InvalidOperationException($"Duplicate {nameof(EntityField)} name: '{propertyDescription.EntityFieldName}'");
                }
            }

            return this;
        }

        private bool IsLettersOnly(string str)
        {
            return !string.IsNullOrWhiteSpace(str) && str.All(c => char.IsLetter(c) || _validSpecialCharacters.Contains(c));
        }

        private bool HasOnePrimaryKey()
        {
            return PropertyDescriptions.Count(property => property.IsPrimaryKey) == 1;
        }
    }
}
