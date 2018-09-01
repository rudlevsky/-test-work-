using System;

namespace DualLabProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter a file path:");

            var obj = new Schedule(Console.ReadLine());
            obj.MakeSorted();

            Console.WriteLine("Enter a path for saving a file:");
            obj.MoveToFile(Console.ReadLine());
        }
    }
}
