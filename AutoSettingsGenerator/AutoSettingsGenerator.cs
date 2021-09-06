using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;

namespace AutoSettingsGenerator
{
    [Generator]
    public class AutoSettingsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // #if DEBUG
            //         if (!Debugger.IsAttached)
            //         {
            //             Debugger.Launch();
            //         }
            // #endif 
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var files = context.AdditionalFiles.Where(f => Path.HasExtension(".json") 
                                                           && Path.GetFileName(f.Path).Contains("AutoSettings"));
            
            foreach (var file in files)
            {
                var propsSource = new StringBuilder();
                var jsonDocument = JsonDocument.Parse(file.GetText().ToString());
                
                foreach (var jsonProperty in jsonDocument.RootElement.EnumerateObject())
                {
                    if (jsonProperty.Value.ValueKind != JsonValueKind.String) continue;
                    propsSource.AppendLine($"public static string {jsonProperty.Name} => \"{jsonProperty.Value.GetString()}\";");
                }

                var className = Path.GetFileName(file.Path).Replace(Path.GetExtension(file.Path), "");
                context.AddSource(className, $@"
                using System;

                namespace AutoSettings
                {{
                    public class {className}
                    {{
                        {propsSource}
                    }}
                }}");
            }
        }
    }
}
