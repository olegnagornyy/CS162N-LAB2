using System;
using System.Collections.Generic;
namespace BinaryNumbersQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter positive number: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Error");
                return;
            }

            Queue<string> queue = new Queue<string>();
            queue.Enqueue("1");

            Console.WriteLine("Binary numbers from 1 to " + n + ":");
            for (int i = 0; i < n; i++)
            {
                string current = queue.Dequeue();
                Console.WriteLine(current);
                queue.Enqueue(current + "0");
                queue.Enqueue(current + "1");
            }
        }
    }
}