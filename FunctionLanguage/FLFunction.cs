using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    /// An implementation of the <see cref="GSC.FunctionLanguage.IFLFunction" /> interface which uses generic delegates for lambda expressions in function calls.
    /// </summary>
    public class FLFunction : IFLFunction
    {
        public Func<object, object[], object> Func
        {
            get;
            set;
        }

        /// <summary>
        ///     Instantiates an FLFunction with the provided delegate or lambda expression to execute.
        /// </summary>
        /// <param name="func">A delegate to execute when this function is called.</param>
        public FLFunction(Func<object, object[], object> func)
        {
            this.Func = func;
        }

        /// <summary>
        /// Calls this particular function.
        /// </summary>
        /// <param name="thisObject">The reference object on which this function is being called. (i.e. from thisObject-&gt;call())</param>
        /// <param name="args">Arguments that are passed within the function call.</param>
        /// <returns>
        /// The return value of the function.
        /// </returns>
        /// <inheritdoc />
        public object Call(object thisObject, object[] args)
        {
            return Func(thisObject, args);
        }
    }
}
