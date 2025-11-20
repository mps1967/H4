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

    }
}
