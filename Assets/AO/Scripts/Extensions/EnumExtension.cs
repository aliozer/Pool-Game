using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AO.Extensions
{
    public static class EnumExtension
    {
        public static string GetStringValue(this Enum @enum)
        {
            string output = "";
            Type type = @enum.GetType();

            FieldInfo fi = type.GetField(@enum.ToString());
            IStringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(IStringValueAttribute), false) as IStringValueAttribute[];

            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

    }
}
