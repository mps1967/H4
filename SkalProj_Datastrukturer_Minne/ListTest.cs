using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkalProj_Datastrukturer_Minne
{
    internal class ListTest
    {
        private List<string> list_ = new();
        private NameDict nd_ = new();
        private string str_to_find_ = string.Empty;
        private int num_to_find_ = 0;

        internal ListTest() { }

        internal bool FullyEqual(string s)
        {
            return str_to_find_.Equals(s);
        }
        internal bool NameEqual(string s)
        {
            var ns = NameDict.Name(s);
            return ns.Equals(str_to_find_);
        }

        internal bool NumEqual(string s)
        {
            var ns = NameDict.Num(s);
            return ns == num_to_find_;
        }

        internal void Add(string s)
        {
            var name_num = nd_.NewNameNum(s);
            list_.Add(name_num);
        }

        private void RemoveAll(List<int> ix)
        {
            foreach (int i in ix)
            {
                list_.RemoveAt(i);
            }
        }
        // TODO: Make a function that does yield return of all matching indeces
        // which then is passed to RemoveAll.
        internal void Remove(string s)
        {
            List<int> ix = [];
            int prev = -1;
            str_to_find_ = s;
            while (true)
            {
                int start = prev + 1;
                int count = list_.Count - start;
                if (count == 0) break;
                int i = list_.FindIndex(start, count, FullyEqual);
                if (i < 0) break;
                ix.Add(i);
                prev = i;
            }
            RemoveAll(ix);
            Console.WriteLine($"{ix.Count} items removed.");
        }
        internal void RemoveName(string s)
        {
            str_to_find_ = NameDict.Name(s);
            List<int> ix = [];
            int prev = -1;
            while (true)
            {
                int start = prev + 1;
                int count = list_.Count - start;
                if (count == 0) break;
                int i = list_.FindIndex(start, count, NameEqual);
                if (i < 0) break;
                ix.Add(i);
                prev = i;
            }
            RemoveAll(ix);
            Console.WriteLine($"{ix.Count} items removed.");
        }
        internal void RemoveNum(int num)
        {
            num_to_find_ = num;
            List<int> ix = [];
            int prev = -1;
            while (true)
            {
                int start = prev + 1;
                int count = list_.Count - start;
                if (count == 0) break;
                int i = list_.FindIndex(start, count, NumEqual);
                if (i < 0) break;
                ix.Add(i);
                prev = i;
            }
            RemoveAll(ix);
            Console.WriteLine($"{ix.Count} items removed.");
        }
        internal void AddBrothers()
        {
            List<string> brothers = [];
            foreach (string s in list_)
            {
                brothers.Add(NameDict.Name(s));
            }
            foreach (string s in brothers)
            {
                Add(s);
            }
        }
        internal string Display()
        {
            List<string> parts = [];
            foreach (var s in list_)
            {
                parts.Add($"{parts.Count}: {NameDict.Norm(s)}");
            }
            if (parts.Count == 0) return "the list is empty";
            return string.Join(", ", parts);
        }

        private void Minus(string input)
        {
            if (input.Length < 2) return;
            string s = input.Substring(1);
            if (!NameDict.Parse(s, out string name, out int num))
            {
                Console.WriteLine("incorrect - command");
                return;
            }
            if (num != 0)
            {
                if (name.Length > 0)
                {
                    Remove($"{name} {num}");
                    return;
                }
                RemoveNum(num);
                return;
            }
            if (name.Length == 0) return;
            RemoveName(name);
            return;
        }
        internal void CountAndCapacity()
        {
            int count = list_.Count;
            int cap = list_.Capacity;
            Console.WriteLine($"Count={count} Capacity={cap}");
        }
        internal static void Examine()
        {
            /*
             * Loop this method until the user inputs something to exit to main menue.
             * Create a switch with cases to push or pop items
             * Make sure to look at the stack after pushing and and poping to see how it behaves
             */
            ListTest lt = new();
            while (true)
            {
                Console.WriteLine("Possible commands:");
                Console.WriteLine("0     --> Return to main menu.");
                Console.WriteLine("+NAME --> add NAME to the list.");
                Console.WriteLine("-NAME --> remove all with NAME.");
                Console.WriteLine("-NUMBER --> remove all with NUMBER.");
                Console.WriteLine("C     --> count and capacity.");
                Console.WriteLine("D     --> Display the list's state.");
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
                        lt.Add(input.Substring(1));
                        continue;
                    case '-':
                        lt.Minus(input);
                        continue;
                    case 'd': goto case 'D';
                    case 'D':
                        Console.WriteLine(lt.Display());
                        continue;
                    case 'c': goto case 'C';
                    case 'C':
                        lt.CountAndCapacity();
                    case 'b': goto case 'B';
                    case 'B':
                        lt.AddBrothers();
                        continue;
                    default:
                        Console.WriteLine("Please enter some valid input.");
                        continue;
                }
            }
        }
    }
}
