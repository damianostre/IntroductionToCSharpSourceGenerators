using Microsoft.CodeAnalysis;

namespace AutoTestGenerator
{
    public static class DiagnosticsDescriptors
    {
        public static readonly DiagnosticDescriptor IncorrectNumberOfParameters
            = new("AUTOTEST001",                  
                "Error",     
                "Incorrect number of parameters",
                "DemoAnalyzer",
                DiagnosticSeverity.Error,
                true);
    }
}