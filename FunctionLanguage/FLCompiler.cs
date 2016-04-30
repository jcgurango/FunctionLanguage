using FunctionLanguage.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunctionLanguage
{
    /// <summary>
    ///     Compiles and executes FL code passed as strings.
    /// </summary>
    public class FLCompiler
    {
        private enum TokenType
        {
            Operator,
            StringIdentifier,
            LeftParenthesis,
            RightParenthesis,
            Invalid,
            Comma
        }

        //The regex tokenizer results:
        //Group 1 - Operators
        //Groups 2 & 3 - String OR identifier.
        //Group 4 - Left Parenthesis
        //Group 5 - Right Parenthesis
        //Group 6 - Comma
        //No group - Invalid.
        internal const string LeftParenthesis = "(";
        internal const string RightParenthesis = ")";
        internal const string Comma = ",";
        internal const string TextRegex = @"\'((?:\\\'|[\S\s])*?)\'|((?:\w|\.)+)";
        internal const string AsOperator = "as";
        internal const string ObjectOperator = "->";
        internal const string UnaryMinus = "-";
        internal const string CatchAllRegexString = "\\S";
        internal const string CaptureGroupRegexString = @"({0})";
        internal const string RegexOR = @"|";

        internal static readonly string EscapedLeftParenthesis = Regex.Escape(LeftParenthesis);
        internal static readonly string EscapedRightParenthesis = Regex.Escape(RightParenthesis);
        internal static readonly string EscapedComma = Regex.Escape(Comma);
        internal static readonly string FullRegexWithoutOperators = TextRegex + RegexOR +
                string.Format(CaptureGroupRegexString, EscapedLeftParenthesis) + RegexOR +
                string.Format(CaptureGroupRegexString, EscapedRightParenthesis) + RegexOR +
                string.Format(CaptureGroupRegexString, EscapedComma) + RegexOR +
                CatchAllRegexString;


        private FLCompilerSettings compilerSettings;
        
        //The current tokenizer.
        private Match matcher;
        private TokenType currentTokenType;

        private Stack<object> evalStack;

        /// <summary>
        ///    The settings for this particular compiler.
        /// </summary>
        public FLCompilerSettings CompilerSettings
        {
            get
            {
                return compilerSettings;
            }
        }

        /// <summary>
        ///     Instantiates a new compiler with default settings.
        /// </summary>
        public FLCompiler()
            : this(new FLCompilerSettings())
        {
        }

        /// <summary>
        ///     Instantiates a new compiler with existing settings.
        /// </summary>
        /// <param name="settings">Settings to load into the compiler.</param>
        public FLCompiler(FLCompilerSettings settings)
        {
            this.compilerSettings = settings;
            this.evalStack = new Stack<object>();
        }

        /// <summary>
        ///     Executes the given piece of FL code.
        /// </summary>
        /// <param name="flString">FL code to execute.</param>
        /// <exception cref="GSC.FunctionLanguage.Exceptions.UnexpectedEndException">Thrown when the stream or string ends before all scopes are terminated.</exception>
        /// <exception cref="GSC.FunctionLanguage.Exceptions.UnexpectedTokenException">Thrown when a token appears where it is not expected.</exception>
        public object Execute(string flString, IFLContext context)
        {
            //Empty the evaluation stack, in case another execution call filled it up.
            evalStack.Clear();
            
            //Start a matcher to tokenize the string.
            matcher = Regex.Match(flString, createRegexString());
            profileCurrentMatch();
            expression(context);

            if (matcher.Success)
            {
                throw new UnexpectedTokenException(matcher.Value, matcher.Index);
            }

            return evalStack.Pop();
        }

        private string currentToken()
        {
            if (!matcher.Success)
            {
                return null;
            }

            for (int i = 1; i < matcher.Groups.Count; i++)
            {
                if (matcher.Groups[i].Success)
                {
                    if (i == 2 || i == 3)
                    {
                        return matcher.Groups[i].Value.Replace("\\'", "'");
                    }

                    return matcher.Groups[i].Value;
                }
            }

            return matcher.Value;
        }

        private bool accept(TokenType type)
        {
            if (currentTokenType == type)
            {
                nextToken();
                return true;
            }

            return false;
        }

        private bool isString()
        {
            return matcher.Groups[2].Success;
        }

        private bool expect(TokenType type)
        {
            if (accept(type))
            {
                return true;
            }

            if (currentToken() == null)
            {
                throw new UnexpectedEndException();
            }
            else
            {
                throw new UnexpectedTokenException(matcher.Value, matcher.Index);
            }
        }

        private void expression(IFLContext context)
        {
            expression(compilerSettings.OperatorStack.Count - 1, context);
        }

        private void expression(int opSetIndex, IFLContext context)
        {
            term(opSetIndex, context);

            while (compilerSettings.OperatorStack[opSetIndex].Contains(currentToken()))
            {
                string currentOp = currentToken();
                nextToken();

                term(opSetIndex, context);

                object rightOperand = evalStack.Pop();
                object leftOperand = evalStack.Pop();

                callOperator(context, currentOp, rightOperand, leftOperand);
            }
        }

        private void callOperator(IFLContext context, string opName, object rightOperand, object leftOperand)
        {

            object result;

            if (opName == AsOperator)
            {
                result = context.TypeConvert(Convert.ToString(rightOperand), leftOperand);
            }
            else
            {
                result = context.Operate(opName, leftOperand, rightOperand);
            }

            evalStack.Push(result);
        }

        private void term(int opSetIndex, IFLContext context)
        {
            //If this is the end of the operator set chain, get the operand already.
            if (opSetIndex == 0)
            {
                bool negate = matcher.Value == UnaryMinus;

                if (negate) nextToken();

                factor(context);

                if (negate)
                {
                    evalStack.Push(-(double)evalStack.Pop());
                }
            }
            else
            {
                //Otherwise, execute the higher operator first.
                expression(opSetIndex - 1, context);
            }
        }

        /// <summary>
        ///     Gets the value of one of the operands of an operation, or just gets the value.
        /// </summary>
        private void factor(IFLContext context)
        {
            //Identifiers/strings can be either a function call or just an argument.
            if (currentTokenType == TokenType.StringIdentifier)
            {
                bool currentTokenIsString = isString();
                string token = advanceAndKeep();

                //It's a function call.
                if (accept(TokenType.LeftParenthesis))
                {
                    //Get all the arguments of the function call.
                    int argumentCount = functionCall(context);
                    
                    //Execute the function within the context.
                    executeFunction(context, false, token, argumentCount);
                }
                else
                {
                    //It's not a function call, just push it onto the evaluation stack.
                    double result;

                    //If this is not explicitly a string, perhaps we can parse it as a number.
                    if (!currentTokenIsString && double.TryParse(token, out result))
                    {
                        evalStack.Push(result);
                    }
                    else
                    {
                        //We can't. Just keep it as a string.
                        evalStack.Push(token);
                    }
                }
            }
            //This is an expectation, as there should be no others.
            else if (expect(TokenType.LeftParenthesis))
            {
                //Get the expression inside the parentheses.
                expression(context);

                //Now that we have an expresssion, the next token must be a closing right parenthesis.
                expect(TokenType.RightParenthesis);
            }

            referencedCalls(context);
        }

        private void referencedCalls(IFLContext context)
        {
            //An expression can include the -> operator to reference the other within a function call.
            while (currentTokenType == TokenType.Operator && currentToken() == ObjectOperator)
            {
                nextToken();

                //Save the function name.
                string token = currentToken();

                //The next statement must be a function call.
                if (expect(TokenType.StringIdentifier) && expect(TokenType.LeftParenthesis))
                {
                    //Get all the arguments of the function call.
                    int argumentCount = functionCall(context);

                    //Call the function with this expression in the stack as the "this" object.
                    executeFunction(context, true, token, argumentCount);
                }
            }
        }

        private string advanceAndKeep()
        {
            //Save the token.
            string token = currentToken();

            //We need to determine whether this is a function call or just an argument.
            nextToken();
            return token;
        }

        private int functionCall(IFLContext context)
        {
            //Count the arguments of this function.
            int argumentCount = 0;
            
            //See if we even need to loop through the expressions.
            bool rightParenthesisEncountered = accept(TokenType.RightParenthesis);

            while (!rightParenthesisEncountered)
            {
                //Get the expression which will be used as an argument.
                expression(context);

                //Accept a comma, in the case of another argument.
                if (accept(TokenType.Comma))
                {
                    //The token position has already advanced, there's no need to do anything.

                }
                else if (expect(TokenType.RightParenthesis))
                {
                    //Otherwise, the only other character acceptable is the right parenthesis.

                    //This statement will break out of the loop.
                    rightParenthesisEncountered = true;
                }

                argumentCount++;
            }

            return argumentCount;
        }

        private void executeFunction(IFLContext context, bool getObjectReference, string functionName, int argumentCount)
        {
            object[] passedArguments = new object[argumentCount];

            //Load the arguments from the stack, in the order they were placed in.
            for (int i = passedArguments.Length - 1; i >= 0; i--)
            {
                passedArguments[i] = evalStack.Pop();
            }

            object objectReference = null;

            if (getObjectReference)
            {
                objectReference = evalStack.Pop();
            }

            //Call the function in the context.
            object returnValue = context.CallFunction(functionName, objectReference, passedArguments);

            //Push the return value back onto the stack.
            evalStack.Push(returnValue);
        }

        /// <summary>
        ///     Advances the compiler to the next matched token.
        /// </summary>
        private void nextToken()
        {
            matcher = matcher.NextMatch();
            profileCurrentMatch();
        }

        private void profileCurrentMatch()
        {
            currentTokenType = getTokenType(matcher);
        }

        /// <summary>
        ///     Profiles the type of the current token, and returns what it is.
        /// </summary>
        /// <returns>The TokenType of the current token.</returns>
        private TokenType getTokenType(Match match)
        {
            if (match.Success)
            {
                if (match.Groups[1].Success)
                {
                    return TokenType.Operator;
                }

                if (match.Groups[2].Success || match.Groups[3].Success)
                {
                    return TokenType.StringIdentifier;
                }

                if (match.Groups[4].Success)
                {
                    return TokenType.LeftParenthesis;
                }

                if (match.Groups[5].Success)
                {
                    return TokenType.RightParenthesis;
                }

                if (match.Groups[6].Success)
                {
                    return TokenType.Comma;
                }
            }

            return TokenType.Invalid;
        }

        /// <summary>
        ///     Creates a regex string which will tokenize bits of FL code.
        /// </summary>
        /// <returns>A regular expression which matches any token, as well as everything else.</returns>
        private string createRegexString()
        {
            return string.Format(CaptureGroupRegexString, compilerSettings.CreateOperatorRegexString()) + RegexOR + FullRegexWithoutOperators;
        }
    }
}
