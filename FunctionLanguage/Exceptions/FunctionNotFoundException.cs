using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage.Exceptions
{
    public class FunctionNotFoundException : RuntimeException
    {
        public FunctionNotFoundException(string functionName)
            : base(functionName) { }
    }
}
