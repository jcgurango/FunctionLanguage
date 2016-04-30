using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     Handles the execution of functions which may be called from FL code.
    /// </summary>
    public interface IFunctionHandler
    {
        /// <summary>
        ///     Calls the function.
        /// </summary>
        /// <param name="functionName">The name or identifier of the function.</param>
        /// <param name="thisObject">The reference object passed through the dereference (->) operator.</param>
        /// <param name="args">Arguments that are passed within the function call.</param>
        /// <returns>The return value of the function.</returns>
        object CallFunction(string functionName, object thisObject, params object[] args);
    }
}
