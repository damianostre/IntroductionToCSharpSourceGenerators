using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MethodBoilerplateGenerator
{
    public class MethodsTracker : ISyntaxContextReceiver
    {
        public List<IMethodSymbol> Methods = new();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is MethodDeclarationSyntax m && m.Identifier.ValueText.StartsWith("__") )
            {
                Methods.Add(context.SemanticModel.GetDeclaredSymbol(m) as IMethodSymbol);
            }
        }
    }
}