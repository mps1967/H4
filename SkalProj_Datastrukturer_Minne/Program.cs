using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SkalProj_Datastrukturer_Minne
{
    class Program
    {
        /// <summary>
        /// The main method, vill handle the menues for the program
        /// </summary>
        /// <param name="args"></param>
        static void Main()
        {

            while (true)
            {
                // Console.Clear();
                Console.WriteLine("Please navigate through the menu by inputting the number \n(1, 2, 3 ,4, 0) of your choice"
                    + "\n1. Examine a List"
                    + "\n2. Examine a Queue"
                    + "\n3. Examine a Stack"
                    + "\n4. CheckParenthesis"
                    + "\n5. Fibbonacci"
                    + "\n0. Exit the application");
                Console.WriteLine("Please enter some input!");
                switch (ReadChar())
                {
                    case '1':
                        ExamineList();
                        break;
                    case '2':
                        ExamineQueue();
                        break;
                    case '3':
                        ExamineStack();
                        break;
                    case '4':
                        CheckParanthesis();
                        break;
                    case '5':
                        Fibbonacci();
                        break;
                    /*
                     * Extend the menu to include the recursive 
                     * and iterative exercises.
                     */
                    case '0':
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please enter some valid input (0, 1, 2, 3, 4, 5)");
                        break;
                }
            }
        }

        static private char ReadChar()
        {
            string? input = Console.ReadLine();
            if (input == null) return '\0';
            input = input.Trim();
            if (input.Length == 0) return '\0';
            return input[0];
        }

        /// <summary>
        /// Examines the datastructure List
        /// </summary>
        static void ExamineList()
        {
            /*
             * Loop this method untill the user inputs something to exit to main menue.
             * Create a switch statement with cases '+' and '-'
             * '+': Add the rest of the input to the list (The user could write +Adam and "Adam" would be added to the list)
             * '-': Remove the rest of the input from the list (The user could write -Adam and "Adam" would be removed from the list)
             * In both cases, look at the count and capacity of the list
             * As a default case, tell them to use only + or -
             * Below you can see some inspirational code to begin working.
            */

            //List<string> theList = new List<string>();
            //string input = Console.ReadLine();
            //char nav = input[0];
            //string value = input.substring(1);

            //switch(nav){...}
            ListTest.Examine();
        }

        /// <summary>
        /// Examines the datastructure Queue
        /// </summary>
        static void ExamineQueue()
        {
            /*
             * Loop this method untill the user inputs something to exit to main menu.
             * Create a switch with cases to enqueue items or dequeue items
             * Make sure to look at the queue after Enqueueing and Dequeueing to see how it behaves
            */
            Q.Examine();
        }

        /// <summary>
        /// Examines the datastructure Stack
        /// </summary>
        static void ExamineStack()
        {
            /*
             * Loop this method until the user inputs something to exit to main menue.
             * Create a switch with cases to push or pop items
             * Make sure to look at the stack after pushing and and poping to see how it behaves
            */
            StackTest.Examine();
        }

        static void CheckParanthesis()
        {
            /*
             * Use this method to check if the paranthesis in a string is Correct or incorrect.
             * Example of correct: (()), {}, [({})],  List<int> list = new List<int>() { 1, 2, 3, 4 };
             * Example of incorrect: (()]), [), {[()}],  List<int> list = new List<int>() { 1, 2, 3, 4 );
             */
            Parenthesis.Check();
        }

        private static int FibNR(int n)
        {
            if (n < 2) return n;
            List<int> fl = [];
            fl.Add(0);
            fl.Add(1);
            for (int i = 2; i <= n; ++i)
            {
                var a = fl[fl.Count - 1];
                var b = fl[fl.Count - 2];
                var c = a + b;
                if (c < 0) return -1;
                fl.Add(c);
            }
            return fl[n];
        }
        private static int FibR(int n)
        {
            if (n < 2) return n;
            int a = FibR(n - 2);
            if (a < 0) return -1;
            int b = FibR(n - 1);
            if (b < 0) return -1;
            int c = a + b;
            if (c < 0) return -1;
            return c;
        }
        static void Fibbonacci()
        {
            Console.WriteLine("Fibbonacci(N) N=");
            string? input = Console.ReadLine() ?? "0";
            if (!int.TryParse(input, out int num)) return;
            int nr = FibNR(num);
            if (num < 40)
            {
                // The recursion is too slow (no caching of previous results)
                int r = FibR(num);
                if (r < 0)
                {
                    Console.WriteLine("integer overflow");
                    return;
                }
                Console.WriteLine($"Recursive Fibbonacci({num})={r}");
            }
            if (nr < 0)
            {
                Console.WriteLine("integer overflow");
                return;
            }
            Console.WriteLine($"Not Recursive Fibbonacci({num})={nr}");
            return;
        }
    }
}