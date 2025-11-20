using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkalProj_Datastrukturer_Minne
{
    internal class NameDict
    {
        internal static readonly char[] WHITESPACE = { ' ', '\t', '\n', '\r' };
        internal NameDict() { }

        private Dictionary<string, int> dict_ = new Dictionary<string, int>();

        // Gets a displayable string that looks like a name for all
        // sorts of input.
        internal static string Norm(string? s)
        {
            if (s == null) return "__NULL__";
            if (s.Length == 0) return "__EMPTY__";
            string t = s.Trim();
            if (t.Length == 0) return "__SPACE__";
            return t;
        }

        // Returns the name from a name-number string or strange string.
        internal static string Name(string? name_num)
        {
            string s = Norm(name_num);
            StringSplitOptions opt =
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries;
            string[] parts = s.Split(WHITESPACE, opt);
            if (parts.Length == 0) return "__SPACE__";
            if (parts[0].Length == 0) return "__SPACE__";
            return parts[0];
        }

        internal static int Num(string? name_num)
        {
            if (name_num == null) return 0;
            string s = name_num.Trim();
            if (s.Length == 0) return 0;
            StringSplitOptions opt =
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries;
            string[] parts = s.Split(WHITESPACE, opt);
            if (parts.Length < 2) return 0;
            parts[0] = string.Empty;
            s = string.Join(string.Empty, parts);
            if (int.TryParse(s, out var num)) return num;
            return 0;
        }

        internal string NewNameNum(string? s)
        {
            string name = Name(s);
            int num = New(name);
            return $"{name} {num}";
        }

        // returns the next number for the name. First is 1.
        private int New(string name)
        {
            if (dict_.TryAdd(name, 1)) return 1;
            if (dict_.TryGetValue(name, out int num))
            {
                ++num;
                dict_[name] = num;
                return num;
            }
            // Unexplainable sytuation.
            Debug.Assert(false);
            return 0;
        }
        public static bool Parse(string? s, out string name, out int num)
        {
            name = string.Empty;
            num = 0;
            if (s == null) return false;
            string t = Norm(s);
            if (t.Length == 0) return false;
            if (int.TryParse(t, out int x))
            {
                if (x >= 0)
                {
                    num = x;
                    return true;
                }
            }
            StringSplitOptions opt =
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries;
            string[] parts = t.Split(WHITESPACE, opt);
            if (parts.Length == 0) return false;
            if (parts[0].Length == 0) return false;
            name = parts[0];
            parts[0] = string.Empty;
            string n = string.Join(string.Empty, parts);
            n = n.Trim();
            if (n == string.Empty) return true;
            if (int.TryParse(n, out int y))
            {
                num = y;
                return true;
            }
            // we only have a name without a number. that's ok.
            return true;
        }

    }
}
