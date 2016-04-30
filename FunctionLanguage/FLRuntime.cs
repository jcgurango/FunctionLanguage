using FunctionLanguage.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    /// A context in which you may register functions to be called. It uses the default type converter and operation handler by default, although custom implementations may be passed as arguments to the constructor.
    /// </summary>
    public class FLRuntime : IFLContext
    {
        private Dictionary<string, IFLFunction> registeredFunctions;

        /// <summary>
        ///     Gets a type converter to be used with the "as" operator.
        /// </summary>
        public ITypeConverter TypeConverter
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Gets an operation handler to be used with any operations.
        /// </summary>
        public IOperationHandler OperationHandler
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Constructs a new set of FL functions, type converters, and operators.
        /// </summary>
        public FLRuntime() : this(DefaultTypeConverter.Instance, DefaultOperationHandler.Instance)
        {
        }

        /// <summary>
        ///     Constructs a new set of FL functions, type converters, and operators with the given type converter.
        /// </summary>
        /// <param name="typeConverter">The type converter to use.</param>
        /// <param name="opHandler">The operation handler to use.</param>
        public FLRuntime(ITypeConverter typeConverter = null, IOperationHandler opHandler = null)
        {
            //Use the type converter.
            if (typeConverter != null)
            {
                TypeConverter = typeConverter;
            }
            else
            {
                TypeConverter = DefaultTypeConverter.Instance;
            }
            
            //Use the operation handler.
            if (opHandler != null)
            {
                OperationHandler = opHandler;
            }
            else
            {
                OperationHandler = DefaultOperationHandler.Instance;
            }

            //Instantiate a new set of registered functions.
            registeredFunctions = new Dictionary<string, IFLFunction>();
        }

        /// <summary>
        ///     Registers the given function into the function set.
        /// </summary>
        /// <param name="functionName">The name or identifier of the function.</param>
        /// <param name="function">Code to run when this function is called.</param>
        public void RegisterFunction(string functionName, IFLFunction function)
        {
            if (registeredFunctions.ContainsKey(functionName))
            {
                throw new FunctionAlreadyExistsException(functionName);
            }

            registeredFunctions[functionName] = function;
        }

        /// <summary>
        ///     Registers the given function into the function set.
        /// </summary>
        /// <param name="functionName">The name or identifier of the function.</param>
        /// <param name="function">Code to run when this function is called.</param>
        public void RegisterFunction(string functionName, Func<object, object[], object> func)
        {
            RegisterFunction(functionName, new FLFunction(func));
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
            return OperationHandler.Operate(opSymbol, a, b);
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
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <inheritdoc />
        public object CallFunction(string functionName, object thisObject, params object[] args)
        {
            if (registeredFunctions.ContainsKey(functionName))
            {
                return registeredFunctions[functionName].Call(thisObject, args);
            }
            else
            {
                throw new FunctionNotFoundException(functionName);
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
            return TypeConverter.TypeConvert(newType, value);
        }
    }
}
