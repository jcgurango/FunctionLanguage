using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     Contains static settings to be passed to an <see cref="GSC.FunctionLanguage.FLCompiler" /> instance.
    /// </summary>
    public class FLCompilerSettings
    {
        private List<List<string>> operatorStack;

        internal List<List<string>> OperatorStack
        {
            get
            {
                return operatorStack;
            }
        }

        /// <summary>
        ///     Creates a new set of compiler settings.
        /// </summary>
        public FLCompilerSettings()
            : this(new FLRuntime())
        {
        }

        /// <summary>
        ///     Creates a new set of compiler settings.
        /// </summary>
        /// <param name="functionSet">A function set to load into the settings.</param>
        public FLCompilerSettings(FLRuntime functionSet)
        {
            //Instantiate the operator stack.
            operatorStack = new List<List<string>>();

            //Push the as operator onto the stack onto the stack.
            PushOperator(FLCompiler.AsOperator);
        }

        /// <summary>
        ///     Pushes an operator onto the top of the operator stack. Operators pushed last will be executed last.
        /// </summary>
        /// <param name="operatorSymbol">The symbol of the operator. This can have multiple symbols.</param>
        /// <param name="equalOrder">If this is true, it will be execute alongside the previous operator in the stack.</param>
        public void PushOperator(string operatorSymbol, bool equalOrder = false)
        {
            //Make sure that the operator stack is not empty, and this operator is not to be executed with the previous in the stack.
            if (!equalOrder || operatorStack.Count == 0)
            {
                //Place a new operator set onto the stack.
                operatorStack.Add(new List<string>());
            }

            //Place the symbol at the last operator set of the stack.
            operatorStack[operatorStack.Count - 1].Add(operatorSymbol);
        }

        /// <summary>
        ///     Combines all operators into a string, including the built-in -> operator.
        /// </summary>
        /// <returns>A regular expression containing all the operators.</returns>
        public string CreateOperatorRegexString()
        {
            //A variable which is set to false after passing the first operator. Operators after the first one are preceeded by a
            //Regex OR (|).
            string operatorRegex = "";
            bool first = true;

            //Loop through all operator sets.
            foreach (List<string> operatorSet in OperatorStack)
            {
                //Loop through all operators in the set.
                foreach (string opSymbol in operatorSet)
                {
                    //If this is the first operator, just set the flag.
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        //Otherwise, append a Regex OR.
                        operatorRegex += FLCompiler.RegexOR;
                    }

                    //Escape the regex string.
                    string escapedOperatorSymbol = Regex.Escape(opSymbol);

                    //Append the escaped symbol.
                    operatorRegex += escapedOperatorSymbol;
                }
            }

            //Return the final regex string.
            return FLCompiler.ObjectOperator + FLCompiler.RegexOR + operatorRegex;
        }
    }
}
