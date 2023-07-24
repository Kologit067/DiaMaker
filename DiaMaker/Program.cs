using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            string ver1 = "0.20.1.5";
            ver1 = ver1.ImplementationVersionFormat();
//            string ver1 = "000000000.0.1.5";
//            string ver1 = "000.000.001.005";
            string ver2 = "000.020.001.005";
            int c = ver1.CompareTo(ver2);
            if (c < 0)
                Console.WriteLine(ver1 + " < " + ver2);
            if (c > 0)
                Console.WriteLine(ver1 + " > " + ver2);
            if (c == 0)
                Console.WriteLine(ver1 + " = " + ver2);
            Console.ReadLine();
        }
    }

    public static class CommonExtensions
    {

        public static String ImplementationVersionFormat(this string value)
        {
            string[] partsOfValue = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Join(".", partsOfValue.Select(s => s.PadLeft(3,'0')));
            return result;
        }

    }

}
