using Microsoft.CodeAnalysis;
using System;

namespace HelloWorldGenerator
{
    [Generator]
    public class HelloWorldGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource(
                "HelloWorldProvider",
                @"public class HelloWorldProvider
                {
                    public static string HelloWorld()
                    {
                        return ""Hello World!!"";
                    }
                }");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
