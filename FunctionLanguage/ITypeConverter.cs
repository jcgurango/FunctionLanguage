using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     Handles conversions of objects from one type to another.
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        ///     Converts an object to another type.
        /// </summary>
        /// <param name="newType">The new type, represented by a string.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>A type-converted object.</returns>
        object TypeConvert(string newType, object value);
    }
}
