using System;

namespace Core.Scripts
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InitializationOrderAttribute : Attribute
    {
        public int Order { get; }
        public InitializationOrderAttribute(int order) => Order = order;
    }
}