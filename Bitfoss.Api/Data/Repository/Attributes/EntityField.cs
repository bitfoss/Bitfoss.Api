using System;

namespace Bitfoss.Api.Data.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EntityField : Attribute
    {
        public string Name { get; set; }

        public bool IsPrimaryKey { get; set; }
    }
}
