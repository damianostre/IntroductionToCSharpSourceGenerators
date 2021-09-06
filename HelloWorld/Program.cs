using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var helloWorld = HelloWorldProvider.HelloWorld();
            Console.WriteLine(helloWorld);
        }
    }
}
