using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day14
{
    public class InsertionRule
    {
        public string Pair { get; }
        public char Element { get; }

        public InsertionRule(string pair, char element)
        {
            Pair = pair;
            Element = element;
        }
        public override int GetHashCode()
        {
            return Pair.GetHashCode();
        }
    }
    public class Polymerizor
    {
        public string Template { get; private set; }
        public Dictionary<string, char> InsertionRules { get; }

        public Polymerizor(string template, Dictionary<string, char> insertionRules)
        {
            Template = template;
            InsertionRules = insertionRules;
        }

        public void Polymerize()
        {
            var sb = new StringBuilder(Template);
            for (var i = Template.Length - 1; i > 0; i--)
            {
                var key = string.Concat(Template[i - 1], Template[i]);
                sb.Insert(i, InsertionRules[key]);
            }
            Template = sb.ToString();
        }

        internal long GetScore()
        {
            var counts = Template.GroupBy(g => g).Select(g => g.Count()).ToArray();
            var max = counts.Max();
            var min = counts.Min();
            return max - min;
        }
        internal long GetScore(int iterations, bool useTestInput)
        {
            var counts = InsertionRules
                .Select(x => x.Value)
                .Distinct()
                .ToDictionary(x => x, y => (long)0);
            var rules = InsertionRules.Select(x => new InsertionRule(x.Key, x.Value)).ToArray();


            var previousIteration = InsertionRules.ToDictionary(x => x.Key, y => (long)0);
            for (var i = Template.Length - 1; i > 0; i--)
            {
                var key = string.Concat(Template[i - 1], Template[i]);
                previousIteration[key]++;
                counts[Template[i]]++;
            }
            counts[Template[0]]++;

            for (var i = 0; i < iterations; i++)
            {
                var currentIteration = InsertionRules.ToDictionary(x => x.Key, y => (long)0);
                foreach(var current in previousIteration) {
                    var currentElement = InsertionRules[current.Key];
                    counts[currentElement] += current.Value;
                    var left = string.Concat(current.Key[0], currentElement);
                    var right = string.Concat(currentElement, current.Key[1]);
                    currentIteration[left] += current.Value;
                    currentIteration[right] += current.Value;
                }
                previousIteration = currentIteration;
            }
            

            if (useTestInput)
            {
                foreach (var count in counts)
                {
                    Console.Write(count.Key);
                    Console.Write(' ');
                    Console.Write(count.Value);
                    Console.WriteLine();
                }
            }
            var max = counts.Max(x => x.Value);
            var min = counts.Min(x => x.Value);

            return max - min;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 14!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var template =
                (useTestInput ? testInput : input)
                    .Split(Environment.NewLine + Environment.NewLine)
                    .First();
            var insertionRules =
                (useTestInput ? testInput : input)
                    .Split(Environment.NewLine + Environment.NewLine)
                    .Last()
                    .Split(Environment.NewLine)
                    .ToDictionary(x => x.Split(" -> ").First(), y => y.Split(" -> ").Last()[0]);
            // Start part 1.
            sw.Start();
            var polymerizor = new Polymerizor(template, insertionRules);
            if (useTestInput) Console.WriteLine(polymerizor.Template);
            for (int i = 0; i < 10; i++)
            {
                polymerizor.Polymerize();
                if (useTestInput && i < 4) Console.WriteLine(polymerizor.Template);
            }
            answer = polymerizor.GetScore();
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");

            sw.Restart();
            polymerizor = new Polymerizor(template, insertionRules);
            answer = polymerizor.GetScore(10, useTestInput);
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks) (refactored)");


            // Start part 2.
            sw.Restart();
            answer = polymerizor.GetScore(40, useTestInput);
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";
        public static string input = @"NCOPHKVONVPNSKSHBNPF

ON -> C
CK -> H
HC -> B
NP -> S
NH -> H
CB -> C
BB -> H
BC -> H
NN -> C
OH -> B
SF -> V
PB -> H
CP -> P
BN -> O
NB -> B
KB -> P
PV -> F
SH -> V
KP -> S
OF -> K
BS -> V
PF -> O
BK -> S
FB -> B
SV -> B
BH -> V
VK -> N
CS -> V
FV -> F
HS -> C
KK -> O
SP -> N
FK -> B
CF -> C
HP -> F
BF -> O
KC -> C
VP -> O
BP -> P
FF -> V
NO -> C
HK -> C
HV -> B
PK -> P
OV -> F
VN -> H
PC -> K
SB -> H
VO -> V
BV -> K
NC -> H
OB -> S
SN -> B
HF -> P
VF -> B
HN -> H
KS -> S
SC -> S
CV -> B
NS -> P
KO -> V
FS -> O
PH -> K
BO -> C
FH -> B
CO -> O
FO -> F
VV -> N
CH -> V
NK -> N
PO -> K
OK -> K
PP -> O
OC -> P
FC -> N
VH -> S
PN -> C
VB -> C
VS -> P
HO -> F
OP -> S
HB -> N
CC -> K
KN -> S
SK -> C
OS -> N
KH -> B
FP -> S
NF -> S
CN -> S
KF -> C
SS -> C
SO -> S
NV -> O
FN -> B
PS -> S
HH -> C
VC -> S
OO -> C
KV -> P";
    }
}
