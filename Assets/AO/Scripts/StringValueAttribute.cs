using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{


    public class StringValueAttribute : Attribute, IStringValueAttribute
    {
        public string Value { get; private set; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }
}
