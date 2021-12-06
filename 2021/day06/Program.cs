using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace day06
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 6!");
            var sw = new Stopwatch();
            var answer = (long)0;
            var useTestInput = false;
            var school = new long[9];
            var initialTimers =
                (useTestInput ? testInput : input)
                    .Split(',')
                    .Select(int.Parse);
            for (var i = 0; i < school.Length; i++)
            {
                school[i] = initialTimers.Count(x => x == i);
            }

            // Start part 1.
            sw.Start();
            for (int day = 0; day < 80; day++)
            {
                LiveAnotherDay(school);
            }
            answer = school.Sum();
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Start();
            for (int i = 80; i < 256; i++)
            {
                LiveAnotherDay(school);
            }
            answer = school.Sum();
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        private static void LiveAnotherDay(long[] school)
        {
            var newBorns = school[0];
            for (var i = 1; i < school.Length; i++)
            {
                school[i - 1] = school[i];
            }
            school[6] += newBorns;
            school[8] = newBorns;
        }

        public static string testInput = @"3,4,3,1,2";
        public static string input = @"5,3,2,2,1,1,4,1,5,5,1,3,1,5,1,2,1,4,1,2,1,2,1,4,2,4,1,5,1,3,5,4,3,3,1,4,1,3,4,4,1,5,4,3,3,2,5,1,1,3,1,4,3,2,2,3,1,3,1,3,1,5,3,5,1,3,1,4,2,1,4,1,5,5,5,2,4,2,1,4,1,3,5,5,1,4,1,1,4,2,2,1,3,1,1,1,1,3,4,1,4,1,1,1,4,4,4,1,3,1,3,4,1,4,1,2,2,2,5,4,1,3,1,2,1,4,1,4,5,2,4,5,4,1,2,1,4,2,2,2,1,3,5,2,5,1,1,4,5,4,3,2,4,1,5,2,2,5,1,4,1,5,1,3,5,1,2,1,1,1,5,4,4,5,1,1,1,4,1,3,3,5,5,1,5,2,1,1,3,1,1,3,2,3,4,4,1,5,5,3,2,1,1,1,4,3,1,3,3,1,1,2,2,1,2,2,2,1,1,5,1,2,2,5,2,4,1,1,2,4,1,2,3,4,1,2,1,2,4,2,1,1,5,3,1,4,4,4,1,5,2,3,4,4,1,5,1,2,2,4,1,1,2,1,1,1,1,5,1,3,3,1,1,1,1,4,1,2,2,5,1,2,1,3,4,1,3,4,3,3,1,1,5,5,5,2,4,3,1,4";
    }
}
