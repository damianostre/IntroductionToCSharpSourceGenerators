using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;

namespace CrudGenerator
{
    [Generator]
    public class CrudGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var dbContextTemplate = Template.Parse(GetTemplateText("CrudGenerator.Templates.DbContextTemplate.txt"));
            var controllerTemplate = Template.Parse(GetTemplateText("CrudGenerator.Templates.ControllerTemplate.txt"));
            var dbSets = new List<object>();
            
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
                var classes = syntaxTree.GetRoot()
                    .DescendantNodesAndSelf()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(HasCrudAttribute)
                    .Select(c => semanticModel.GetDeclaredSymbol(c))
                    .OfType<INamedTypeSymbol>()
                    .Select(t => new { entityname = t.Name, entitypath = t.ToDisplayString()})
                    .ToList();
                
                classes.ForEach(c =>
                {
                    dbSets.Add(c);
                    context.AddSource(c.entityname + "Controller.gen.cs", controllerTemplate.Render(c));
                });
            }
            
            context.AddSource("CrudDbContext.gen.cs", dbContextTemplate.Render(new {entities = dbSets}));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // #if DEBUG
            //         if (!Debugger.IsAttached)
            //         {
            //             Debugger.Launch();
            //         }
            // #endif 
        }

        private bool HasCrudAttribute(ClassDeclarationSyntax classNode) =>
            classNode.AttributeLists
                .SelectMany(x => x.Attributes)
                .Any(x => x.Name.ToString() == nameof(CrudEntityAttribute)
                          || x.Name.ToString() == "CrudEntity");

        private string GetTemplateText(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
