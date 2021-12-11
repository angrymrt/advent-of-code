using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace day11
{
    class Node
    {
        public int Value { get; private set; }
        public int X { get; }
        public int Y { get; }
        public Node[][] Map { get; }
        public Node TopLeft => X == 0 || Y == 0 ? null : Map[X - 1][Y - 1];
        public Node Top => X == 0 ? null : Map[X - 1][Y];
        public Node TopRight => X == 0 || Y == Map[X].Length - 1 ? null : Map[X - 1][Y + 1];
        public Node Left => Y == 0 ? null : Map[X][Y - 1];
        public Node Right => Y == Map[X].Length - 1 ? null : Map[X][Y + 1];
        public Node BottomLeft => X == Map.Length - 1 || Y == 0 ? null : Map[X + 1][Y - 1];
        public Node Bottom => X == Map.Length - 1 ? null : Map[X + 1][Y];
        public Node BottomRight => X == Map.Length - 1 || Y == Map[X].Length - 1 ? null : Map[X + 1][Y + 1];
        public IEnumerable<Node> Neighbours => new Node[] { TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight }.Where(x => x != null);

        public void NextStep()
        {
            Value++;
            CheckForFlash();
        }
        public int Reset()
        {
            if (Value > 9)
            {
                Value = 0;
                return 1;
            }
            return 0;
        }
        private void CheckForFlash()
        {
            if (Value == 10)
            {
                foreach (var node in Neighbours)
                {
                    if (node.Value < 10)
                    {
                        node.Value++;
                        node.CheckForFlash();
                    }
                }
            }
        }

        public Node(int value, int x, int y, Node[][] map)
        {
            Value = value;
            X = x;
            Y = y;
            Map = map;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 11!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var lines =
                (useTestInput ? testInput : input)
                    .Split(Environment.NewLine)
                    .Select(x => x.Select(c => int.Parse(c.ToString())).ToArray())
                    .ToArray();

            // Start part 1.
            sw.Start();
            var map = new Node[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                map[i] = new Node[lines[i].Length];
                for (var j = 0; j < lines[i].Length; j++)
                {
                    map[i][j] = new Node(lines[i][j], i, j, map);
                }
            }
            var allNodes = map.SelectMany(x => x).ToArray();
            var steps = 100;
            for (int i = 0; i < steps; i++)
            {
                foreach (var node in allNodes)
                {
                    node.NextStep();
                }
                foreach (var node in allNodes)
                {
                    answer += node.Reset();
                }
            }
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            answer = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                map[i] = new Node[lines[i].Length];
                for (var j = 0; j < lines[i].Length; j++)
                {
                    map[i][j] = new Node(lines[i][j], i, j, map);
                }
            }
            allNodes = map.SelectMany(x => x).ToArray();
            while (allNodes.Any(x => x.Value > 0))
            {
                answer++;
                foreach (var node in allNodes)
                {
                    node.NextStep();
                }
                foreach (var node in allNodes)
                {
                    node.Reset();
                }
            }

            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";
        public static string input = @"2138862165
2726378448
3235172758
6281242643
4256223158
1112268142
1162836182
1543525861
1882656326
8844263151";
    }
}
