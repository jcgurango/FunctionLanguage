using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     Handles the execution of a particular function called from FL code.
    /// </summary>
    public interface IFLFunction
    {
        /// <summary>
        ///     Calls this particular function.
        /// </summary>
        /// <param name="thisObject">The reference object on which this function is being called. (i.e. from thisObject->call())</param>
        /// <param name="args">Arguments that are passed within the function call.</param>
        /// <returns>The return value of the function.</returns>
        object Call(object thisObject, object[] args);
    }
}
