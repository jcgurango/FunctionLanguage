using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     Handles operations between two operands.
    /// </summary>
    public interface IOperationHandler
    {
        /// <summary>
        ///     Execute the operation.
        /// </summary>
        /// <param name="opSymbol">The symbol used in the operation.</param>
        /// <param name="a">The left operand.</param>
        /// <param name="b">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        object Operate(string opSymbol, object a, object b);
    }
}
