using System;

namespace AutoTestGenerator
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoTestAttribute : Attribute
    {
        public AutoTestAttribute(object expectedResult, params object[] parameters)
        {
            
        }
    }
}