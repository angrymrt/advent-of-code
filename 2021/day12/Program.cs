using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace day12
{
    public class Path : List<Cave>
    {
        public Path() : base() { }
        public Path(IEnumerable<Cave> collection) : base(collection) { }

        public bool IsComplete => Count > 1 && this[Count - 1].IsEnd;

        public override string ToString()
        {
            var result = "";
            for (int i = 0; i < Count; i++)
            {
                result += "," + this[i].Name;
            }
            return result.Length == 0 ? result : result.Substring(1);
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
    public class Cave
    {
        public static Regex Upper = new Regex("[A-Z]{1,}");
        public string Name { get; }
        public Tuple<string, string>[] Lines { get; }
        public List<Cave> Caves { get; }
        public bool IsBig { get; }
        public bool IsSmall { get; }
        public bool IsStart { get; }
        public bool IsEnd { get; }
        public IEnumerable<Cave> ConnectedCaves => Caves.Where(x => 
            x.Name != Name 
            && !x.IsStart 
            && Lines.Any(l => 
                (l.Item1 == Name && l.Item2 == x.Name) 
                || (l.Item2 == Name && l.Item1 == x.Name)));
        public IEnumerable<Path> GetPaths(Path path)
        {
            if (path.IsComplete)
            {
                yield return path;
            }
            else
            {
                var cur = path.Last();
                foreach (var cave in cur.ConnectedCaves)
                {
                    if (cave.IsBig || cave.IsEnd || (cave.IsSmall && !path.Exists(x => x.Name == cave.Name)))
                    {
                        var newPath = new Path(path);
                        newPath.Add(cave);
                        foreach (var result in GetPaths(newPath))
                        {
                            yield return result;
                        }
                    }
                }
            }
        }
        public IEnumerable<Path> GetPathsPart2(Path path)
        {
            if (path.IsComplete)
            {
                yield return path;
            }
            else
            {
                var cur = path.Last();
                foreach (var cave in cur.ConnectedCaves)
                {
                    if (cave.IsBig 
                    || cave.IsEnd 
                    || (cave.IsSmall && !path.Exists(x => x.Name == cave.Name)) 
                    || (cave.IsSmall && path.Count(x => x.IsSmall) == path.Distinct().Count(x => x.IsSmall) && path.Count(x => x.Name == cave.Name) < 2))
                    {
                        var newPath = new Path(path);
                        newPath.Add(cave);
                        foreach (var result in GetPathsPart2(newPath))
                        {
                            yield return result;
                        }
                    }
                }
            }
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public Cave(string name, Tuple<string, string>[] lines, List<Cave> caves)
        {
            Name = name;
            Lines = lines;
            Caves = caves;
            IsBig = Upper.IsMatch(name);
            IsSmall = !IsBig;
            IsStart = name == "start";
            IsEnd = name == "end";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 12!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var lines =
                (useTestInput ? testInput : input)
                    .Split(Environment.NewLine)
                    .Select(x => new Tuple<string, string>(x.Split('-').First(), x.Split('-').Last()))
                    .ToArray();

            // Start part 1.
            sw.Start();
            var caves = new List<Cave>();
            caves.AddRange(lines
                .SelectMany(x => new string[] { x.Item1, x.Item2 })
                .Distinct()
                .Select(x => new Cave(x, lines, caves)));
            var start = caves.First(x => x.IsStart);
            var startPath = new Path(new Cave[] { start });
            var paths = start.GetPaths(startPath).ToArray();
            var distinctPaths = paths.Distinct().ToArray();
            if (useTestInput)
            {
                foreach (var path in distinctPaths)
                {
                    Console.WriteLine(path.ToString());

                }
            }
            answer = distinctPaths.Count();
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            answer = 0;
            paths = start.GetPathsPart2(startPath).ToArray();
            distinctPaths = paths.Distinct().ToArray();
            if (useTestInput)
            {
                foreach (var path in distinctPaths)
                {
                    Console.WriteLine(path.ToString());

                }
            }
            answer = distinctPaths.Count();
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";
        public static string input = @"by-TW
start-TW
fw-end
QZ-end
JH-by
ka-start
ka-by
end-JH
QZ-cv
vg-TI
by-fw
QZ-by
JH-ka
JH-vg
vg-fw
TW-cv
QZ-vg
ka-TW
ka-QZ
JH-fw
vg-hu
cv-start
by-cv
ka-cv";
    }
}
