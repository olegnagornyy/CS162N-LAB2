using System;
using System.Collections.Generic;

namespace StringReverseStack
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<char> stack = new Stack<char>();

            Console.WriteLine("Please enter a set of characters into the console. It will be printed in reverse.");

            string input = Console.ReadLine();

            for(int i = 0; i < input.Length; i++ )
            {
                char n = input[i];
                stack.Push(n);
            }

            Console.WriteLine("Here is the output: ");
                foreach (char n in stack)
                    Console.Write(n + "");
        }
    }
}
