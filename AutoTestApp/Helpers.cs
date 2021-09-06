using AutoTestGenerator;
using Xunit;

namespace AutoTestApp
{
    public class Helpers
    {
        [AutoTest(expectedResult: 14, 6, 9)]
        public static int Add(int a, int b)
        {
            return a + b;
        }
        
        [AutoTest(expectedResult: 3)]
        public static int Get3()
        {
            return 3;
        }
    }
}