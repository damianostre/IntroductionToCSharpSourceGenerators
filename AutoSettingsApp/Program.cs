using System;
using AutoSettings;

namespace AutoSettingsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = DefaultAutoSettings.Env;
            Console.WriteLine(test);
        }
    }
}