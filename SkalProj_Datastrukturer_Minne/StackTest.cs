using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkalProj_Datastrukturer_Minne
{
    internal class StackTest
    {
        private Stack<string> stack_ = new();
        private NameDict nd_ = new();

        internal StackTest() { }
        internal void Push(string s)
        {
            var name_num = nd_.NewNameNum(s);
            stack_.Push(name_num);
        }
        internal string? Pop()
        {
            if (stack_.TryPop(out string result)) return result;
            return null;
        }
        internal string Display()
        {
            List<string> parts = [];
            foreach (var s in stack_)
            {
                int i = parts.Count;
                parts.Add($"{i}: {NameDict.Norm(s)}");
            }
            if (parts.Count == 0) return "The stack is empty.";
            return string.Join(", ", parts);
        }
        internal void PushBrothers()
        {
            List<string> brothers = [];
            foreach (var s in stack_)
            {
                brothers.Add(NameDict.Name(s));
            }
            foreach (var s in brothers)
            {
                Push(s);
            }
        }
        internal static void Examine()
        {
            /*
             * Loop this method until the user inputs something to exit to main menue.
             * Create a switch with cases to push or pop items
             * Make sure to look at the stack after pushing and and poping to see how it behaves
             */
            StackTest st = new StackTest();
            while (true)
            {
                Console.WriteLine("Possible commands:");
                Console.WriteLine("0     --> Return to main menu.");
                Console.WriteLine("+NAME --> push NAME to the stack.");
                Console.WriteLine("-     --> pop someone.");
                Console.WriteLine("D     --> Display the stack's state.");
                Console.WriteLine("B     --> Push everyone's brother/sister.");
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
                        st.Push(input.Substring(1));
                        continue;
                    case '-':
                        string? served = st.Pop();
                        if (served == null)
                        {
                            Console.WriteLine("the stack was empty.");
                            continue;
                        }
                        Console.WriteLine($"{served} is out.");
                        continue;
                    case 'd': goto case 'D';
                    case 'D':
                        Console.WriteLine(st.Display());
                        continue;
                    case 'b': goto case 'B';
                    case 'B':
                        st.PushBrothers();
                        continue;
                    default:
                        Console.WriteLine("Please enter some valid input.");
                        continue;
                }
            }
        }
    }
}
