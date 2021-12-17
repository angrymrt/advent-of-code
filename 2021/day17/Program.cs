using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day17
{
    public class Target
    {
        public Point Start { get; }
        public Point End { get; }

        public Target(string input)
        {
            var target = input.Replace("target area: x=", "")
                            .Split(", y=")
                            .SelectMany(x => x.Split(".."))
                            .Select(int.Parse)
                            .ToArray();
            Start = new Point(target[0], target[3]);
            End = new Point(target[1], target[2]);
        }
    }
    public class Probe
    {
        public Point Location { get; private set; } = new Point(0, 0);
        public Point HighestLocation { get; private set; } = new Point(0, 0);
        public bool ReachedTarget { get; private set; }
        public Point InitialVelocity { get; private set; }

        public Probe(Point velocity)
        {
            InitialVelocity = velocity;
        }

        public Point Step(Point velocity, Target target)
        {
            Location = new Point(Location.X + velocity.X, Location.Y + velocity.Y);
            if (Location.Y >= HighestLocation.Y)
            {
                HighestLocation = Location;
            }
            if (IsInTarget(target))
            {
                ReachedTarget = true;
            }
            var newVelocityX = 0;
            if (velocity.X > 0)
            {
                newVelocityX = velocity.X - 1;
            }
            else if (velocity.X < 0)
            {
                newVelocityX = velocity.X + 1;
            }

            return new Point(newVelocityX, velocity.Y - 1);
        }
        public bool IsInTarget(Target target)
        {
            return Location.X >= target.Start.X
                && Location.X <= target.End.X
                && Location.Y <= target.Start.Y
                && Location.Y >= target.End.Y;
        }
        public bool IsOnTarget(Target target, Point velocity)
        {
            return Location.X <= target.End.X && Location.Y >= target.End.Y;
        }
    }
    public class ProbeLauncher
    {
        public static Probe Launch(Point velocity, Target target)
        {
            var probe = new Probe(velocity);
            var currentVelocity = velocity;
            while (probe.IsOnTarget(target, currentVelocity))
            {
                currentVelocity = probe.Step(currentVelocity, target);
            }
            return probe;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 17!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var target = new Target(useTestInput ? testInput : input);


            // Start part 1.
            sw.Start();
            var maxX = target.End.X;
            for (var x = 1; x < maxX; x++)
            {
                if (HasPotential(x, target))
                {
                    var bestProbe = (Probe)null;
                    var probe = (Probe)null;
                    for (var y = 1; y <= 0 - target.End.Y; y++)
                    {
                        probe = ProbeLauncher.Launch(new Point(x, y), target);
                        if (probe.ReachedTarget && (bestProbe == null || bestProbe.HighestLocation.Y < probe.HighestLocation.Y))
                        {
                            bestProbe = probe;
                            if (useTestInput)
                            {
                                Console.WriteLine($"Current best for x = {x}: Velocity[{x},{bestProbe.InitialVelocity.Y}], HighestY[{bestProbe.HighestLocation.Y}]");
                            }
                        }
                    }
                    if (bestProbe != null && answer < bestProbe.HighestLocation.Y)
                    {
                        answer = bestProbe.HighestLocation.Y;
                        if (useTestInput)
                        {
                            Console.WriteLine($"Best for x = {x}: Velocity[{x},{bestProbe.InitialVelocity.Y}], HighestY[{bestProbe.HighestLocation.Y}]");
                        }
                    }
                }
            }
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            var initialVelocities = new List<Point>();
            for (var x = 1; x <= target.End.X; x++)
            {
                if (HasPotential(x, target))
                {
                    for (var y = target.End.Y; y <= 0 - target.End.Y; y++)
                    {
                        var velocity = new Point(x, y);
                        var probe = ProbeLauncher.Launch(velocity, target);
                        if (probe.ReachedTarget)
                        {
                            initialVelocities.Add(velocity);
                        }
                    }
                }
            }
            answer = initialVelocities.Count();
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        private static bool HasPotential(int x, Target target)
        {
            var i = 0;
            while (x > 0)
            {
                i += x;
                if (i >= target.Start.X && i <= target.End.X)
                {
                    return true;
                }
                x--;
            }
            return false;
        }

        public static string testInput = @"target area: x=20..30, y=-10..-5";
        public static string input = @"target area: x=287..309, y=-76..-48";
    }
}
