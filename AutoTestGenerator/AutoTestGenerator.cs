using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTestGenerator
{
    [Generator]
    public class AutoTestGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // #if DEBUG
            //             if (!Debugger.IsAttached)
            //             {
            //                 Debugger.Launch();
            //             }
            // #endif 

            context.RegisterForSyntaxNotifications(() => new AutoTestMethodsTracker());
        }

        
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not AutoTestMethodsTracker tracker) return;

            var groupedMethods = tracker.AutoTestMethods.GroupBy(m => m.ContainingType.Name);

            foreach (var method in groupedMethods)
            {
                var funcsSource = string.Empty;

                foreach (var func in method)
                {
                    var autoTestAttrs = func
                        .GetAttributes()
                        .Where(a => a.AttributeClass?.Name == nameof(AutoTestAttribute))
                        .ToList();

                    foreach (var autoTest in autoTestAttrs)
                    {
                        var expectedResult = autoTest.ConstructorArguments[0].ToCSharpString();
                        var parameters = autoTest.ConstructorArguments[1].Values
                            .Select(v => v.ToCSharpString())
                            .ToArray();

                        if (parameters.Length != func.Parameters.Length)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                DiagnosticsDescriptors.IncorrectNumberOfParameters, 
                                autoTest.ApplicationSyntaxReference.GetSyntax().GetLocation()));
                        }

                        var i = autoTestAttrs.IndexOf(autoTest) + 1;
                        funcsSource += $@"
                            [Fact]
                            public void {func.Name}Test{i}()
                            {{
                                Assert.Equal({expectedResult}, {func.ContainingType}.{func.Name}({string.Join(", ", parameters)}));
                            }}";
                    }
                }

                var className = method.First().ContainingType.Name + "Tests";
                context.AddSource(className, $@"
                    using System;
                    using Xunit;

                    namespace {method.First().ContainingNamespace.Name}.AutoTests
                    {{
                        public class {className}
                        {{
                            {funcsSource}
                        }}
                    }}");
            }
        }
    }
}