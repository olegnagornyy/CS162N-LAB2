using System;
using System.Collections.Generic;
namespace BinaryNumbersQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter string to reverse: ");
            string input = Console.ReadLine();
            Stack<char> stack = new Stack<char>();

            foreach (char ch in input)
            {
                stack.Push(ch);
            }

            Console.Write("Reversed: ");
            while (stack.Count > 0)
            {
                Console.Write(stack.Pop());
            }
            Console.WriteLine();
        }
    }
}