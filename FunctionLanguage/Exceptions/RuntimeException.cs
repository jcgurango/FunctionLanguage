using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage.Exceptions
{
    public class RuntimeException : Exception
    {
        public RuntimeException()
            : base()
        {
        }
        public RuntimeException(string message)
            : base(message)
        {
        }
    }
}
