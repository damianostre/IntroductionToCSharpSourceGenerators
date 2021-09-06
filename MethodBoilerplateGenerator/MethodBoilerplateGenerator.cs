using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace MethodBoilerplateGenerator
{
    [Generator]
    public class MethodBoilerplateGenerator: ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MethodsTracker());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is MethodsTracker tracker)) return;

            foreach (var grouping in tracker.Methods.GroupBy(m => m.ContainingType.Name))
            {
                var classSymbol = grouping.First().ContainingType;
                var methodsSource = new StringBuilder();
                
                foreach (var methodSymbol in grouping)
                {
                    methodsSource.AppendLine($@"
                    public {methodSymbol.ReturnType} {methodSymbol.Name.TrimStart('_')}(
                        {string.Join(", ", methodSymbol.Parameters.Select(p => $"{p.Type} {p.Name}"))})
                    {{       
                        Console.WriteLine(""Started execution of {methodSymbol.Name}"");
                        { (!methodSymbol.ReturnsVoid ? "var result =" : "") } 
                            {methodSymbol.Name}({string.Join(", ", methodSymbol.Parameters.Select(p => p.Name))});
                        Console.WriteLine(""Finished execution of {methodSymbol.Name}"");
                        { (!methodSymbol.ReturnsVoid ? "return result;" : "") } 
                    }}");
                }
                
                context.AddSource(classSymbol.Name, $@"
                    using System;

                    namespace {classSymbol.ContainingNamespace.Name}
                    {{
                        public partial class {classSymbol.Name}
                        {{
                            {methodsSource}
                        }}
                    }}");
            }
        }
    }
}
