using System;
using System.Collections.Generic;
namespace BinaryNumbersQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<string> queque = new Queue<string>();

            int n = GetInt("The number you want to express as a binary number", 1, 1000);

            queque.Enqueue("1");

            for (int i = 1; i <= n; i++)
            {
                string next = queque.Dequeue();
                queque.Enqueue(next + "0");
                queque.Enqueue(next + "1");
                Console.WriteLine(next);

            }

            Console.ReadLine();

        }

        public static int GetInt(string label, int min, int max)
        {
            bool isInt = false;
            int number = min - 1;
            do
            {
                Console.Write(String.Format("Please enter a whole number between {0} and {1} for {2}: ", min, max, label));
                string input = Console.ReadLine();
                isInt = int.TryParse(input, out number);
            } while (!(isInt && number >= min && number <= max));

            return number;
        }
    }
}
