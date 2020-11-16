using System;

namespace Bitfoss.Api.Data.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Entity : Attribute
    {
        public string Name { get; set; }
    }
}
