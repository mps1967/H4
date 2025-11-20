using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SkalProj_Datastrukturer_Minne
{
    /// <summary>
    /// Checks if parenthesis are well opened and closed.
    /// Dragon book approach:
    /// S : P S | _
    /// P : P1 | P2 | P3 | P4
    /// P1 : ( S )
    /// P2 : [ S ]
    /// P3 : { S }
    /// P4 : < S >
    /// 
    /// _ : end of string.
    /// Any char not a paranthesis or EOS is eliminated by the lexer.
    /// 
    /// first(S): "([{<"
    /// follow(S): ")]}>_"
    /// first(P1): "("
    /// follow(P1) : first(s) 
    /// first(P1): "("
    /// follow(P1) : first(s) 
    /// first(P2: "["
    /// follow(P2) : first(s) 
    /// first(P3): "{"
    /// follow(P3) : first(s) 
    /// </summary>
    /// first(P4): "{"
    /// follow(P4) : first(s) 
    /// <param name="s">string to check.</param>
    internal class Parenthesis
    {
        // 1MB of stack, 12000 was determined experimentally.
        // --> ~70 bytes / stack frame in debug mode.
        // --> huge! --> why?!
        public int MAX_INPUT_LENGTH = 12000;  // allow the member to be set.
        private string s_;
        private int current_ = 0;
        private bool error_ = false;
        private static readonly char EOS = '\0';  // just a coincidence,
                                                  // not going to consider
                                                  // the string as
                                                  // null-terminated.
        private static readonly string first_P = "([{<";

        internal Parenthesis(string s)
        {
            s_ = s;
            error_ = !SanityCheck();
        }
        // Empty if no error.
        internal string Error()
        {
            if (!error_) return string.Empty;
            string err = "EOS";
            if (current_ != s_.Length) err = s_.Substring(current_);
            return $"{ s_} : position {current_}: unexpected {err}";
        }
        private char Current()
        {
            while (current_ != s_.Length)
            {
                char p = s_[current_];
                if ("()[]{}<>".Contains(p)) return p;
                ++current_;
            }
            return EOS;
        }
        private bool Advance()
        {
            if (current_ == s_.Length) return false;
            ++current_;
            return true;
        }
        private static char End(char first)
        {
            switch (first)
            {
                case '(': return ')';
                case '[': return ']';
                case '{': return '}';
                case '<': return '>';
                default: throw new ArgumentOutOfRangeException(nameof(first));
            }
        }

        // Method called in constructor. If sanity checks fail Parse() throws.

        // Maybe split the string in parts with balanced parenthesis that
        // Parse() can check separately thus reducing recursion.
        // Not done because it does not help in the worst case scenario.
        private bool SanityCheck()
        {
            int round = 0;
            int square = 0;
            int brace = 0;
            int angular = 0;
            while (Current() != EOS)
            {
                switch (Current())
                {
                    case '(':
                        ++round;
                        break;
                    case ')':
                        --round;
                        if (round < 0) return false;
                        break;
                    case '[':
                        ++square;
                        break;
                    case ']':
                        --square;
                        if (square < 0) return false;
                        break;
                    case '{':
                        ++brace;
                        break;
                    case '}':
                        --brace;
                        if (brace < 0) return false;
                        break;
                    case '<':
                        ++angular;
                        break;
                    case '>':
                        --angular;
                        if (angular < 0) return false;
                        break;
                    default:
                        break;
                }
                Debug.Assert(Advance());  // loop is on Current() != EOS.
            }
            if (round != 0) return false;
            else if (square != 0) return false;
            else if (brace != 0) return false;
            else if (angular != 0) return false;
            return true;
        }
        internal bool Parse()
        {
            if (error_) return false;  // failed SanityCheck().
            // we are using a recoursive algorithm.
            // before calling into the recursive function do a sanity check to
            // avoid stack overflow. The recursive parser is still needed for
            // something like (<)>. As is, the algorithm must not be deployed
            // to process attacker's choice of input at least for fear of
            // denial of service attacks. This the reason for checking the
            // string's length. Imagine 1GB of openning parenthesis given to
            // the recursive algorithm.
            if (s_.Length > MAX_INPUT_LENGTH)  // MAX_INPUT_LENGTH can be set but
                                               // a larger value risks stack
                                               // overflow.
            {
                // dangerous to parse a very long string recursively.
                // TODO: Improve the algorithm, use a stack instead of the call stack.
                throw new ArgumentOutOfRangeException(nameof(s_));
            }
            current_ = 0;
            error_ = !Parse_S(EOS);
            return !error_;
        }
        private bool Parse_P()
        {
            char first = Current();
            if (!first_P.Contains(first)) return false;
            if (!Advance()) return false;
            char end = End(first);
            if (!Parse_S(end)) return false;
            if (Current() != end) return false;
            if (!Advance()) return false;
            return true;
        }
        private bool Parse_S(char end)
        {
            while (Current() != end)
            {
                if (!Parse_P()) return false;
            }
            return true;
        }

        internal static void Check()
        {
            var p1 = new Parenthesis("(())");
            Debug.Assert(p1.Error().Length == 0);
            Debug.Assert(p1.Parse());
            var p2 = new Parenthesis("{}");
            Debug.Assert(p2.Error().Length == 0);
            Debug.Assert(p2.Parse());
            var p3 = new Parenthesis("[({})]");
            Debug.Assert(p3.Error().Length == 0);
            Debug.Assert(p3.Parse());
            var p4 = new Parenthesis("List<int> list = new List<int>() { 1, 2, 3, 4 };");
            Debug.Assert(p4.Parse());
            var p5 = new Parenthesis("(()), {}, [({})],  List<int> list = new List<int>() { 1, 2, 3, 4 };");
            Debug.Assert(p5.Parse());
            int SMAX = 6000;  // 6000 deep recursion works,
                              // 7500 leads to stack overflow.
            string open = new string('(', SMAX);
            string close = new string(')', SMAX);
            string smax = open + close;
            var p6 = new Parenthesis(smax);
            p6.MAX_INPUT_LENGTH = smax.Length;
            Debug.Assert(p6.Parse());
            var w1 = new Parenthesis("(()])");
            Debug.Assert(!w1.Parse());
            Console.WriteLine(w1.Error());
            var w2 = new Parenthesis("[)");
            Debug.Assert(!w2.Parse());
            Console.WriteLine(w2.Error());
            var w3 = new Parenthesis("{[()}]");
            Debug.Assert(!w3.Parse());
            Console.WriteLine(w3.Error());
            var w4 = new Parenthesis("List<int> list = new List<int>() { 1, 2, 3, 4 );");
            Debug.Assert(!w4.Parse());
            Console.WriteLine(w4.Error());
            Console.Write("Parantes checkning:");
            string text = Console.ReadLine() ?? string.Empty;
            var v = new Parenthesis(text);
            if (v.Parse()) Console.WriteLine("Parenteser är OK.");
            else Console.WriteLine(v.Error());
        }
    }
}
