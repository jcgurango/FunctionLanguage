using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage.Exceptions
{
    public class UnexpectedTokenException : CompilationException
    {
        public UnexpectedTokenException(string token, int charPosition)
            : base(string.Format("Token \"{0}\", Position {1}", token, charPosition))
        {
        }
    }
}
