using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace day13
{
    class Program
    {
        static int Solve1(string input, bool verbose = false){
            var earliestPossibleBus = int.Parse(input.Split(Environment.NewLine).First());
            var bussesInService = input.Split(Environment.NewLine)
                                        .Skip(1)
                                        .First()
                                        .Split(',')
                                        .Where(x => x != "x")
                                        .Select(int.Parse);
            var orderedByEarliest = bussesInService.Select(x => new { Bus = x, Closest = x - earliestPossibleBus % x })
                                                    .OrderBy(x => x.Closest);
            if(verbose){
                foreach(var x in orderedByEarliest) {
                    Console.WriteLine($"Bus: {x.Bus}, {x.Closest}");
                }
            }
            var earliest = orderedByEarliest.First();
            return earliest.Bus * earliest.Closest;
        }
        private static IEnumerable<bool> Infinite()  
        {  
            while (true)  
            {  
                yield return true;  
            }  
        }
        static long Solve2(string input, bool verbose = false){
            var answer = 0L;

            var list = input.Split(',')
                            .Select((x, i) => new { Index = i, Bus = x == "x" ? 0 : int.Parse(x) })
                            .Where(x => x.Bus > 0)
                            .ToArray();

            if(verbose) {
                Console.WriteLine(list.Select(x => $"{x.Index} mod({x.Bus})" ).Aggregate((x, y) => $"{x},{y}"));
            }

            long increment = list.First().Bus;
            var currentBusIndex = 1;
            var currentbus = list[currentBusIndex];
            while(true) {
                //if(verbose) Console.WriteLine($"{currentbus.Bus} - {answer} % {currentbus.Bus} = {currentbus.Bus - answer % currentbus.Bus} = {currentbus.Index}");
                if(currentbus.Index == currentbus.Bus - answer % currentbus.Bus
                    || currentbus.Index % currentbus.Bus == currentbus.Bus - answer % currentbus.Bus) {
                    currentBusIndex++;
                    if(currentBusIndex == list.Count()) break;
                    increment *= currentbus.Bus;
                    currentbus = list[currentBusIndex];
                    if(verbose) Console.WriteLine($"New increment: {list.Take(currentBusIndex).Select(x => x.Bus.ToString()).Aggregate((x, y) => $"{x}*{y}")} = {increment}");
                }

                answer += increment;
                //if(answer > 4000) break;
            }
            return answer;
        }
        static long Solve2BruteForce(string input, long start = 0, bool verbose = false){
            var list = input.Split(',')
                            .Select((x, i) => new { Index = i, BusLine = x })
                            .Where(x => x.BusLine != "x")
                            .Select(x => new { Index = x.Index, BusLine = int.Parse(x.BusLine)});
            long answer = start;
            var tracker = list.FirstOrDefault();
            Parallel.ForEach(Infinite(), new ParallelOptions() { MaxDegreeOfParallelism = 8 }, (line, state, index) =>
            {
                var first = list.FirstOrDefault(x => {
                        switch(x.Index) {
                            case 0:
                                return x.Index != index % x.BusLine;
                            default:
                                return x.Index != x.BusLine - index % x.BusLine;
                        }
                    });
                if(first == null) {
                    answer = index;
                    state.Break();
                }
            });
            return answer;
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"Answer part 1 (test input): {Solve1(testInput, true)}");
            Console.WriteLine($"Answer part 1: {Solve1(input)}");

            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2("17,x,13,19", false)}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");

            sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2("7,13,x,x,59,x,31,19", false)}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");
            
            sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2("67,7,59,61", false)}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");

            sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2("67,x,7,59,61")}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");

            sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2("67,7,x,59,61")}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");

            sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2("1789,37,47,1889", false)}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");
            
            sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Answer part 2 (test input): {Solve2(input.Split(Environment.NewLine).Last(), true)}");
            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed}");
        }

        private static string testInput = @"939
        7,13,59,31,19";
        private static string input = @"1005595
41,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,557,x,29,x,x,x,x,x,x,x,x,x,x,13,x,x,x,17,x,x,x,x,x,23,x,x,x,x,x,x,x,419,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,19"; 
    }
}
