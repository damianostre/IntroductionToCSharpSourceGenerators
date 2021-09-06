using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTestGenerator
{
    public class AutoTestMethodsTracker : ISyntaxContextReceiver
    {
        public List<IMethodSymbol> AutoTestMethods = new();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is MethodDeclarationSyntax m && HasAutoTestAttribute(m))
            {
                AutoTestMethods.Add(context.SemanticModel.GetDeclaredSymbol(m) as IMethodSymbol);
            }
        }

        private bool HasAutoTestAttribute(MethodDeclarationSyntax method) =>
            method.AttributeLists
                .SelectMany(x => x.Attributes)
                .Any(x => x.Name.ToString() == nameof(AutoTestAttribute)
                          || x.Name.ToString() == "AutoTest");
    }
}