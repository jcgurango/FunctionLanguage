using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage.Exceptions
{
    public class FunctionAlreadyExistsException : RuntimeException
    {
        public FunctionAlreadyExistsException(string functionName)
            : base(functionName)
        {
        }
    }
}
