using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     A singleton <see cref="GSC.FunctionLanguage.ITypeConverterS"/> which handles conversions to and from DateTimes (datetime), Doubles (number), Strings (string), and Booleans (boolean).
    /// </summary>
    public class DefaultTypeConverter : ITypeConverter
    {
        private static DefaultTypeConverter instance;

        private DefaultTypeConverter() { }

        /// <summary>
        ///     An instance of the type converter to be used anywhere.
        /// </summary>
        public static DefaultTypeConverter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultTypeConverter();
                }

                return instance;
            }
        }

        /// <summary>
        /// Converts an object to another type.
        /// </summary>
        /// <param name="newType">The new type, represented by a string.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// A type-converted object.
        /// </returns>
        /// <inheritdoc />
        public object TypeConvert(string newType, object value)
        {
            switch (newType.ToLower())
            {
                case "number":
                    return Convert.ToDouble(value);
                case "datetime":
                    return Convert.ToDateTime(value);
                case "string":
                    return Convert.ToString(value);
                case "boolean":
                    return Convert.ToBoolean(value);
            }

            return null;
        }
    }
}
