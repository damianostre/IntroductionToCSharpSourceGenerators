using System;

namespace MethodBoilerplateApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Helpers();
            Console.WriteLine(a.ToUpper("Hello Wolrd"));
            
            a.Write("Konferencja");
            a.Write("IT");
        }
    }

    public partial class Helpers
    {
        public void Write(string s)
        {
            Console.WriteLine(s);
        }
        
        public string ToUpper(string a)
        {
            return a.ToUpper();
        }
    }
    
    public partial class Foo
    {
        //Tutaj definicja
        public partial void Bar();
    }
    
    public partial class Foo
    {
        //Tutaj implementacja
        public partial void Bar()
        {
            Console.WriteLine("Foo.Bar");
        }
    }
}
