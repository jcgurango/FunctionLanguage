using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     A context to be passed to the compiler for execution.
    /// </summary>
    public interface IFLContext : IFunctionHandler, ITypeConverter, IOperationHandler
    {
    }
}
