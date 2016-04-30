using FunctionLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLanguage.Test
{
    public class Program
    {
        private const string testScript = @"";
        private static Random r = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
        {
            //This will run these with default settings.
            FLCompilerSettings settings = new FLCompilerSettings();
            settings.PushOperator("*");
            settings.PushOperator("/", true);
            settings.PushOperator("+");
            settings.PushOperator("-", true);
            settings.PushOperator(">=");
            settings.PushOperator("<=", true);
            settings.PushOperator(">", true);
            settings.PushOperator("<", true);
            settings.PushOperator("==");
            settings.PushOperator("&&");
            settings.PushOperator("||");

            FLCompiler compiler = new FLCompiler(settings);
            FLRuntime runtime = new FLRuntime();

            //Register some functions into the runtime.
            runtime.RegisterFunction("concat", (t, x) =>
            {
                return String.Concat(x);
            });

            runtime.RegisterFunction("join", (t, x) =>
            {
                if (x.Length <= 1)
                {
                    return null;
                }

                return String.Join(x[0].ToString(), x.Skip(1));
            });

            runtime.RegisterFunction("repeat", (t, x) =>
            {
                if (x.Length != 2)
                {
                    return null;
                }

                string repeatedText = "";
                int count = Convert.ToInt32(x[1]);

                for (int i = 0; i < count; i++)
                {
                    repeatedText += x[0];
                }

                return repeatedText;
            });

            runtime.RegisterFunction("add", (t, x) =>
            {
                double total = t == null ? 0f : (double)t;

                foreach (object obj in x)
                {
                    total += Convert.ToSingle(obj);
                }

                return total;
            });

            runtime.RegisterFunction("subtract", (t, x) =>
            {
                double total = t == null ? 0f : (double)t;

                foreach (object obj in x)
                {
                    total -= Convert.ToDouble(obj);
                }

                return total;
            });

            runtime.RegisterFunction("floor", (t, x) =>
            {
                return Math.Floor(Convert.ToDouble(x[0]));
            });

            runtime.RegisterFunction("rand", (t, x) =>
            {
                if (x.Length == 2)
                {
                    return r.Next(Convert.ToInt32(x[0]), Convert.ToInt32(x[1]));
                }

                if (x.Length == 0)
                {
                    return r.NextDouble();
                }

                return null;
            });

            //Set the last read line to run the test script, assuming there is one.
            string readLine = testScript;

            Console.WriteLine("Type exit to quit.");

            //Main loop.
            do
            {
                try
                {
                    if (!string.IsNullOrEmpty(readLine))
                    {
                        Console.WriteLine(compiler.Execute(readLine, runtime));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Caught:");
                    Console.WriteLine(e.GetType().ToString() + ": " + e.Message);
                }

                Console.Write("> ");
            }
            while ((readLine = Console.ReadLine()) != "exit");
        }
    }
}
