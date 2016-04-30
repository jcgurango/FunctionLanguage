# Function Language
## What is it?
Function language (or FL for short) is a scripting language and compiler for .NET which allows you to run arbitrary scripts which execute .NET code. The scripts just execute functions and operators. Literals in the scripts adhere to no types, they're all strings. You completely define the functions of the application as well as all the operators with the exception of the "as" operator, which is the built-in cast operator.

## Syntax
The syntax of the scripting language is C-like. You can execute functions like this:

```
foo()
```

You can execute operators like this:

```
10 + 10 + 10
```

There are only two types of literals which scripts can contain, strings and numbers. Numbers don't need any special constructs to put in, but strings need to be placed between the single quote character (') to work:

```
10.015 + 'somestring'
```

It's important to note that even though numbers are written without quotes around them, they're still processed as strings internally. The in-built "as" operator can cast anything to whatever type you'd like:

```
10 as 'boolean'
```

FL also has a dereference operator which you can use in case something returns an object which can have functions run on it:

```
foo('bar')->someFuncWhichCanBeExecutedOnIt()
```

Nested expressions also work:
```
10 + (15 * 30) + (foo('bar') * foo('bar')->foo())
```

In practice, you define the functions which can be used, the operators which can be used, the order of the operations, and even what "types" exist. With regards to the "as" operator, that's built in. However, you decide what happens when a script uses it.

## Try it out
If you want to test out the scripting language itself, you can run the project "FunctionLanguage.CommandLine" which will provide you a command line in which you can type in scripts to be executed by FL. There are 7 built-in functions in that example.

1. **concat(arg0, arg1, argN)** - Concatenates any arguments you place in it.
2. **join(delimiter, arg0, arg1, argN)** - Concatenates all the "args" using the first argument to separate them.
3. **repeat(text, n)** - Repeats the given text *n* times.
4. **add(arg0, arg1, argN)** - Casts and then does a SUM on all the arguments.
5. **subtract(arg0, arg1, argN)** - Casts and then does a subtraction on all the arguments from each other.
6. **floor(num)** - Floors the given number.
7. **rand(min, max) or rand()** - Either returns a random integer between min and max or returns a random double.

In addition to this, you can also use basic arithmetic functions (+, -, /, *) and the dereference operator (->). Also, there are some types you can cast to using the **as** operator which represent .NET types.

1. number - System.Double
1. datetime - System.DateTime
1. string - System.String
1. boolean - System.Boolean

Hopefully that would be enough to prove its power. You can use that project to see how the order of operations is determined and ultimately how to set up Function Language in an application. That should be enough to get you going.

## In-Depth
There's 2 components to any FL instance.

1. The runtime - This contains all the function definitions, operators, and what happens when the as operator is used.
2. The compiler - This is used to compile FL code.

The compiler is fairly self explanatory. You pass it a runtime object and a script, and as it compiles the script it executes functions and operations using the runtime. The runtime, however, has 3 components.

1. Function Handler - This handles the calling of functions. Every time the script calls a function, it passes this the name of the function, the "this" object (in case the -> operator is used), and the arguments.
2. Type Converter - This handles what happens when a script uses the "as" operator. It passes this component the new type to cast to, and the object on which it was called.
3. Operation Handler - This handles what happens when operations are used. Except, of course, for the deference operator and the as operator.

All of these three components combine to form what's called an FL context, which is represented by the *IFLContext* interface. The FL compiler uses this context to call functions, convert types, and execute operations.

I wouldn't recommend building your own *IFLContext* from scratch, unless you have requirements which demand it. Instead, I'd recommend using the *FLRuntime* class. This class allows you to define a type converter and operation handler *if you'd like*, but otherwise it uses the default implementations. Then, it leaves up to you what functions are included (using lambda expressions), and the order of operations. You can see this in action in the FunctionLanguage.CommandLine project. It has constructors which allow you to define which *ITypeConverter* or *IOperationHandler* (or both) implementation you want to use, in case the default ones just aren't cutting in.

Hopefully that's enough detail. I'll put up more if people think it's not enough, but FL itself is pretty simple.