using System;
using System.Collections.Generic;
using System.Linq;

namespace day10
{
    class Program
    {
        static (int part1, long part2) CalculateAnswers(string input) {
            var data = input.Split(Environment.NewLine)
                                .Select(int.Parse)
                                .OrderBy(x => x);
            // Linked lists are my favorite new collection :-P
            var adapters = new LinkedList<int>(data);
            // your device's built-in adapter is always 3 higher as the last
            adapters.AddLast(adapters.Last.Value + 3);
            // you need to read the assignment carefully, because all sequences start with 0..
            adapters.AddFirst(0);

            var cursor = adapters.First.Next;
            var joltDifferenceCounts = new Dictionary<int, int>();
            joltDifferenceCounts[1] = 0;
            joltDifferenceCounts[3] = 0; 
            var answerPart2 = (long)1;
            var currentConsequetive1 = 0;
            var factor = 1;

            while(cursor != null) {
                var difference = cursor.Value - cursor.Previous.Value;
                joltDifferenceCounts[difference] += 1;
                if(difference == 1) {
                    currentConsequetive1++;
                    factor += currentConsequetive1;
                } else {
                    if(currentConsequetive1 > 1) {
                        Console.Write($"[{cursor.Value}: {answerPart2}*{factor - currentConsequetive1}=");
                        answerPart2 *= (factor - currentConsequetive1);
                        Console.WriteLine($"{answerPart2}]");
                    }
                    currentConsequetive1 = 0;
                    factor = 1;
                }
                cursor = cursor.Next;
            }
            var answerPart1 = joltDifferenceCounts[1] * joltDifferenceCounts[3];

            return (answerPart1, answerPart2);
        }
        static void Main(string[] args)
        {
            var answer = CalculateAnswers(shortTestInput);
            Console.WriteLine($"Answer for day 10, part 2 short test input: {answer.part2}");

            answer = CalculateAnswers(mediumTestInput);
            Console.WriteLine($"Answer for day 10, part 2 medium test input: {answer.part2}");
            
            answer = CalculateAnswers(input);
            Console.WriteLine($"Answer for day 10, part 1: {answer.part1}");
            Console.WriteLine($"Answer for day 10, part 2: {answer.part2}");
        }

        private static string shortTestInput = @"16
10
15
5
1
11
7
19
6
12
4";
        private static string mediumTestInput = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";
        private static string input = @"56
139
42
28
3
87
142
57
147
6
117
95
2
112
107
54
146
104
40
26
136
127
111
47
8
24
13
92
18
130
141
37
81
148
31
62
50
80
91
33
77
1
96
100
9
120
27
97
60
102
25
83
55
118
19
113
49
133
14
119
88
124
110
145
65
21
7
74
72
61
103
20
41
53
32
44
10
34
121
114
67
69
66
82
101
68
84
48
73
17
43
140";
    }
}
