using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineInject
{
    class GeneratedProperty
    {
        public Type PropertyType { get; private set; }
        public string Name { get; private set; }
        public object PropertyValue { get; private set; }

        public GeneratedProperty(Type propertyType, string name, object value)
        {
            PropertyType = propertyType;
            Name = name;
            PropertyValue = value;
        }
    }
}
