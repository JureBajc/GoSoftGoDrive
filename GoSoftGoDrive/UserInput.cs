using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSoftGoDrive;


namespace GosoftGoDrive
{
    public static class UserInput
    {
        public static string GetAlgorithmChoice()
        {
            Console.WriteLine("Izberi algoritem:");
            Console.WriteLine("1 - Prikaz sledi korakov");
            Console.WriteLine("2 - Prikaz vseh korakov");
            Console.Write("Vnesi izbiro: ");
            return Console.ReadLine();
        }
    }
}