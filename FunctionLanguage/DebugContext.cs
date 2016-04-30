using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    /// An <see cref="GSC.FunctionLanguage.IFLContext" /> that does no actual function calls, type conversions, or operations. It will simply print out to the console any operations as they happen. You can also hook onto its events.
    /// </summary>
    public class DebugContext : IFLContext
    {
        /// <summary>
        /// Occurs when an operation must be run between two operands.
        /// </summary>
        public event Action<string, object, object> Operated;

        /// <summary>
        /// Occurs when a function must be called.
        /// </summary>
        public event Action<string, object, object[]> FunctionCalled;

        /// <summary>
        /// Occurs when a type must be converted.
        /// </summary>
        public event Action<string, object> TypeConverted;

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
            Console.WriteLine(a.ToString() + " " + opSymbol + " " + b.ToString());

            if (Operated != null)
            {
                Operated(opSymbol, a, b);
            }

            return "the result";
        }

        /// <summary>
        /// Calls the function.
        /// </summary>
        /// <param name="functionName">The name or identifier of the function.</param>
        /// <param name="thisObject">The reference object passed through the dereference (-&gt;) operator.</param>
        /// <param name="args">Arguments that are passed within the function call.</param>
        /// <returns>
        /// The return value of the function.
        /// </returns>
        /// <inheritdoc />
        public object CallFunction(string functionName, object thisObject, params object[] args)
        {
            Console.WriteLine(functionName + " with " + args.Length + " arguments and \"this\" equalling " + (thisObject ?? "nothing").ToString() + ".");
            
            if (FunctionCalled != null)
            {
                FunctionCalled(functionName, thisObject, args);
            }
            
            return "returned value";
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
            Console.WriteLine("Convert " + value.ToString() + " to " + newType + ".");

            if (TypeConverted != null)
            {
                TypeConverted(newType, value);
            }

            return "Converted Object";
        }
    }
}
