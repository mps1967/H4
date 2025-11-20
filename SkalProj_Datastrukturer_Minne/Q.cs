using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkalProj_Datastrukturer_Minne
{
    internal class Q
    {
        private Queue<string> queue_ = new();
        private NameDict nd_ = new();
        internal Q() { }

        internal void Enqueue(string s)
        {
            var name_num = nd_.NewNameNum(s);
            queue_.Enqueue(name_num);
        }
        internal string? Dequeue()
        {
            if (queue_.TryDequeue(out string? result)) return result;
            return null;
        }

        internal string Display()
        {
            List<string> parts = [];
            foreach (var s in queue_)
            {
                int i = parts.Count;
                parts.Add($"{i}: {NameDict.Norm(s)}");
            }
            if (parts.Count == 0) return "The queue is empty.";
            return string.Join( ", ", parts );
        }
        
        internal void EnqueueBrothers()
        {
            int n = queue_.Count;
            // man får det här om man ändra kön medan itereras:
            // System.InvalidOperationException: Collection was modified;
            // enumeration operation may not execute.
            List<string> brothers = new List<string>();
            foreach (var s in queue_)
            {
                brothers.Add(NameDict.Name(s));
            }
            foreach (var s in brothers)
            {
                Enqueue(s);
            }
        }
        internal static void Examine()
        {
            /*
             * Loop this method untill the user inputs something to exit to main menu.
             * Create a switch with cases to enqueue items or dequeue items
             * Make sure to look at the queue after Enqueueing and Dequeueing to see how it behaves
            */
            Q q = new Q();
            while (true)
            {
                Console.WriteLine("Possible commands:");
                Console.WriteLine("0     --> Return to main menu.");
                Console.WriteLine("+NAME --> add NAME to the queue.");
                Console.WriteLine("-     --> Serve someone.");
                Console.WriteLine("D     --> Display the queue state.");
                Console.WriteLine("B     --> Add everyone's brother/sister.");
                string input = Console.ReadLine() ?? string.Empty;
                char f = '\0';
                if (input.Length > 0) f = input[0];
                switch (f)
                {
                    case '0': return;
                    case '+':
                        if (input.Length < 2)
                        {
                            Console.WriteLine("a name is required after +");
                            continue;
                        }
                        q.Enqueue(input.Substring(1));
                        continue;
                    case '-':
                        string? served = q.Dequeue();
                        if (served == null)
                        {
                            Console.WriteLine("the queue was empty.");
                            continue;
                        }
                        Console.WriteLine($"{served} is out.");
                        continue;
                    case 'd':
                        goto case 'D';
                    case 'D':
                        Console.WriteLine(q.Display());
                        continue;
                    case 'b':
                        goto case 'B';
                    case 'B':
                        q.EnqueueBrothers();
                        continue;
                    default:
                        Console.WriteLine("Please enter some valid input.");
                        continue;
                }
            }
        }
    }
}
