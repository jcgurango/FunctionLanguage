using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     A singleton <see cref="GSC.FunctionLanguage.IOperationHandler" /> which handles basic arithmetic operations (add, subtract, divide, multiply), value comparisons (greater/less than, equal to, etc.), and boolean operators (and, or).
    /// </summary>
    public class DefaultOperationHandler : IOperationHandler
    {
        private static DefaultOperationHandler instance;

        private DefaultOperationHandler() { }

        /// <summary>
        ///     An instance of the operation handler to be used anywhere.
        /// </summary>
        public static DefaultOperationHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultOperationHandler();
                }

                return instance;
            }
        }

        /// <summary>
        /// Execute the operation.
        /// </summary>
        /// <param name="opSymbol">The symbol used in the operation.</param>
        /// <param name="a">The left operand.</param>
        /// <param name="b">The right operand.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <inheritdoc />
        public object Operate(string opSymbol, object a, object b)
        {
            switch (opSymbol)
            {
                case "+":
                    return Convert.ToDouble(a) + Convert.ToDouble(b);
                case "-":
                    return Convert.ToDouble(a) - Convert.ToDouble(b);
                case "*":
                    return Convert.ToDouble(a) * Convert.ToDouble(b);
                case "/":
                    return Convert.ToDouble(a) / Convert.ToDouble(b);
                case ">":
                    return Convert.ToDouble(a) > Convert.ToDouble(b);
                case "<":
                    return Convert.ToDouble(a) < Convert.ToDouble(b);
                case ">=":
                    return Convert.ToDouble(a) >= Convert.ToDouble(b);
                case "<=":
                    return Convert.ToDouble(a) <= Convert.ToDouble(b);
                case "==":
                    return a.Equals(b);
                case "||":
                    return Convert.ToBoolean(a) || Convert.ToBoolean(b);
                case "&&":
                    return Convert.ToBoolean(a) && Convert.ToBoolean(b);
            }

            return null;
        }
    }
}
