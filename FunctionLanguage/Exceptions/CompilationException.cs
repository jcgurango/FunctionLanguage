﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage.Exceptions
{
    public class CompilationException : Exception
    {
        public CompilationException()
            : base()
        {
        }

        public CompilationException(string message)
            : base(message)
        {
        }
    }
}
