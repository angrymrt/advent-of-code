using System.Text;

internal class GiantCargoCrane9001 : GiantCargoCrane
{
    public GiantCargoCrane9001(string input) : base(input) { }

    public override void ExecuteInstructions()
    {
        foreach (var instruction in Instructions)
        {
            var cratesToMove = new List<char>(instruction.Move);
            for (var i = 0; i < instruction.Move; i++)
            {
                cratesToMove.Add(Cargo.Stacks[instruction.From - 1].Pop());
            }
            for (var i = instruction.Move - 1; i >= 0; i--)
            {
                Cargo.Stacks[instruction.To - 1].Push(cratesToMove[i]);
            }
        }
    }
}
internal class GiantCargoCrane9000 : GiantCargoCrane
{
    public GiantCargoCrane9000(string input) : base(input) { }

    public override void ExecuteInstructions()
    {
        foreach (var instruction in Instructions)
        {
            for (var i = 0; i < instruction.Move; i++)
            {
                Cargo.Stacks[instruction.To - 1].Push(Cargo.Stacks[instruction.From - 1].Pop());
            }
        }
    }
}

internal abstract class GiantCargoCrane
{
    public readonly Cargo Cargo;
    public readonly Instruction[] Instructions;

    public GiantCargoCrane(string input)
    {
        var splitInput = input.Split(Environment.NewLine + Environment.NewLine);
        Cargo = new Cargo(splitInput[0]);
        Instructions = splitInput[1]
            .Split(Environment.NewLine)
            .Select(x => new Instruction(x))
            .ToArray();
    }

    public abstract void ExecuteInstructions();
}

internal class Cargo
{
    public Stack<char>[] Stacks { get; }

    public Cargo(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var numberOfStacks = (lines[0].Length + 1) / 4;
        Stacks = new Stack<char>[numberOfStacks];
        for (var s = 0; s < numberOfStacks; s++) Stacks[s] = new Stack<char>();
        for (var i = lines.Length - 2; i >= 0; i--)
        {
            for (var s = 0; s < numberOfStacks; s++)
            {
                var crate = lines[i][1 + (s * 4)];
                if (crate != ' ')
                {
                    Stacks[s].Push(crate);
                }
            }
        }
    }

    public override string ToString()
    {
        var maxStackCount = Stacks.Max(x => x.Count());
        var maxStackIndex = maxStackCount - 1;
        var result = new StringBuilder();
        for (var i = maxStackIndex; i >= 0; i--)
        {
            for (var s = 0; s < Stacks.Length; s++)
            {
                if (Stacks[s].Count() >= i + 1)
                {
                    result.Append(Stacks[s].ElementAt(Stacks[s].Count() - i - 1));
                }
                else
                {
                    result.Append(' ');
                }
            }
            result.AppendLine();
        }
        return result.ToString();
    }
}

internal class Instruction
{
    public readonly int Move;
    public readonly int From;
    public readonly int To;

    public Instruction(string input)
    {
        var parts = input.Split(' ');
        Move = int.Parse(parts[1]);
        From = int.Parse(parts[3]);
        To = int.Parse(parts[5]);
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2022, day 3");

        var crane = new GiantCargoCrane9000(input);
        crane.ExecuteInstructions();

        var sb = new StringBuilder();
        var answerPart1 = sb.Append(crane.Cargo.Stacks.Select(x => x.Peek()).ToArray()).ToString();
        Console.WriteLine($"Answer part 1: {answerPart1}");

        var crane2 = new GiantCargoCrane9001(input);
        crane2.ExecuteInstructions();

        sb.Clear();
        var answerPart2 = sb.Append(crane2.Cargo.Stacks.Select(x => x.Peek()).ToArray()).ToString();
        Console.WriteLine($"Answer part 2: {answerPart2}");

    }

    static string testInput = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    static string input = @"[T]     [D]         [L]            
[R]     [S] [G]     [P]         [H]
[G]     [H] [W]     [R] [L]     [P]
[W]     [G] [F] [H] [S] [M]     [L]
[Q]     [V] [B] [J] [H] [N] [R] [N]
[M] [R] [R] [P] [M] [T] [H] [Q] [C]
[F] [F] [Z] [H] [S] [Z] [T] [D] [S]
[P] [H] [P] [Q] [P] [M] [P] [F] [D]
 1   2   3   4   5   6   7   8   9 

move 3 from 8 to 9
move 2 from 2 to 8
move 5 from 4 to 2
move 7 from 1 to 4
move 3 from 8 to 2
move 3 from 2 to 7
move 1 from 7 to 4
move 3 from 2 to 9
move 4 from 7 to 9
move 1 from 5 to 2
move 2 from 3 to 4
move 5 from 9 to 5
move 6 from 9 to 3
move 5 from 9 to 5
move 1 from 9 to 7
move 2 from 3 to 1
move 7 from 3 to 9
move 2 from 7 to 2
move 5 from 2 to 4
move 1 from 2 to 9
move 2 from 1 to 9
move 7 from 6 to 1
move 2 from 7 to 3
move 2 from 3 to 9
move 1 from 7 to 4
move 1 from 9 to 2
move 3 from 1 to 8
move 2 from 3 to 4
move 5 from 9 to 2
move 1 from 3 to 9
move 8 from 5 to 7
move 1 from 6 to 1
move 15 from 4 to 1
move 4 from 2 to 5
move 5 from 9 to 7
move 1 from 9 to 5
move 5 from 1 to 2
move 3 from 8 to 9
move 1 from 7 to 6
move 11 from 1 to 2
move 7 from 5 to 3
move 4 from 2 to 6
move 7 from 3 to 4
move 3 from 5 to 9
move 2 from 2 to 5
move 5 from 1 to 8
move 2 from 6 to 8
move 3 from 8 to 9
move 9 from 4 to 9
move 9 from 7 to 4
move 2 from 8 to 1
move 1 from 8 to 7
move 6 from 2 to 7
move 5 from 2 to 4
move 5 from 7 to 2
move 2 from 1 to 7
move 2 from 6 to 4
move 7 from 7 to 1
move 3 from 2 to 6
move 1 from 8 to 7
move 2 from 9 to 3
move 2 from 3 to 1
move 1 from 2 to 5
move 4 from 6 to 5
move 2 from 2 to 3
move 3 from 5 to 7
move 1 from 5 to 3
move 9 from 1 to 7
move 2 from 9 to 5
move 13 from 4 to 1
move 5 from 7 to 2
move 3 from 3 to 1
move 2 from 2 to 9
move 1 from 2 to 7
move 5 from 5 to 6
move 2 from 2 to 4
move 5 from 1 to 3
move 9 from 7 to 8
move 2 from 9 to 5
move 3 from 5 to 4
move 5 from 9 to 2
move 10 from 4 to 8
move 1 from 4 to 1
move 2 from 8 to 4
move 4 from 8 to 2
move 3 from 6 to 8
move 7 from 8 to 7
move 10 from 9 to 3
move 7 from 3 to 2
move 11 from 2 to 3
move 13 from 3 to 9
move 1 from 6 to 3
move 1 from 1 to 2
move 1 from 2 to 8
move 3 from 3 to 4
move 1 from 2 to 9
move 1 from 4 to 1
move 10 from 8 to 3
move 11 from 9 to 7
move 1 from 6 to 2
move 14 from 7 to 1
move 2 from 2 to 9
move 4 from 7 to 6
move 1 from 2 to 4
move 3 from 4 to 2
move 4 from 2 to 9
move 10 from 3 to 4
move 3 from 6 to 1
move 5 from 9 to 5
move 5 from 5 to 8
move 1 from 9 to 7
move 2 from 9 to 6
move 1 from 9 to 8
move 2 from 4 to 8
move 1 from 4 to 5
move 2 from 3 to 1
move 2 from 3 to 7
move 27 from 1 to 2
move 2 from 7 to 1
move 9 from 4 to 6
move 9 from 6 to 5
move 5 from 8 to 6
move 26 from 2 to 3
move 1 from 2 to 5
move 1 from 2 to 7
move 1 from 8 to 4
move 1 from 7 to 8
move 24 from 3 to 5
move 1 from 8 to 5
move 1 from 4 to 3
move 1 from 7 to 1
move 1 from 8 to 9
move 7 from 1 to 7
move 8 from 6 to 4
move 4 from 7 to 6
move 1 from 3 to 9
move 2 from 9 to 1
move 3 from 7 to 9
move 8 from 4 to 6
move 3 from 9 to 1
move 1 from 3 to 6
move 1 from 8 to 2
move 10 from 5 to 4
move 1 from 3 to 8
move 13 from 5 to 3
move 1 from 2 to 9
move 1 from 8 to 9
move 1 from 3 to 8
move 1 from 9 to 2
move 3 from 6 to 9
move 7 from 4 to 9
move 4 from 3 to 9
move 2 from 6 to 8
move 2 from 4 to 5
move 10 from 9 to 3
move 1 from 1 to 9
move 1 from 4 to 8
move 1 from 1 to 4
move 1 from 4 to 5
move 4 from 6 to 3
move 1 from 9 to 5
move 1 from 6 to 9
move 2 from 6 to 5
move 1 from 9 to 2
move 1 from 6 to 7
move 18 from 5 to 2
move 22 from 3 to 7
move 19 from 7 to 1
move 3 from 8 to 5
move 4 from 9 to 3
move 2 from 7 to 2
move 1 from 8 to 1
move 19 from 1 to 3
move 2 from 7 to 5
move 13 from 3 to 9
move 4 from 1 to 2
move 3 from 5 to 1
move 11 from 9 to 1
move 11 from 2 to 8
move 3 from 9 to 3
move 3 from 5 to 2
move 2 from 1 to 4
move 5 from 2 to 7
move 12 from 1 to 5
move 2 from 4 to 5
move 9 from 5 to 8
move 1 from 5 to 3
move 4 from 2 to 3
move 2 from 7 to 5
move 6 from 2 to 8
move 17 from 8 to 9
move 2 from 9 to 6
move 2 from 7 to 1
move 15 from 9 to 6
move 2 from 2 to 4
move 9 from 8 to 5
move 2 from 1 to 3
move 12 from 6 to 2
move 2 from 3 to 9
move 5 from 6 to 3
move 4 from 5 to 3
move 11 from 3 to 4
move 2 from 9 to 4
move 6 from 5 to 2
move 13 from 4 to 3
move 1 from 4 to 5
move 1 from 4 to 8
move 18 from 2 to 6
move 2 from 5 to 3
move 1 from 8 to 3
move 1 from 2 to 5
move 1 from 7 to 8
move 28 from 3 to 6
move 2 from 3 to 4
move 3 from 5 to 9
move 2 from 5 to 9
move 3 from 9 to 3
move 5 from 3 to 4
move 1 from 9 to 3
move 1 from 9 to 1
move 1 from 3 to 4
move 45 from 6 to 2
move 1 from 8 to 3
move 2 from 4 to 6
move 5 from 4 to 2
move 1 from 3 to 7
move 3 from 2 to 9
move 1 from 4 to 8
move 3 from 6 to 1
move 42 from 2 to 8
move 2 from 9 to 2
move 4 from 2 to 6
move 2 from 2 to 7
move 1 from 9 to 6
move 2 from 8 to 9
move 4 from 1 to 8
move 1 from 6 to 4
move 1 from 4 to 8
move 1 from 2 to 5
move 3 from 7 to 4
move 39 from 8 to 3
move 7 from 8 to 5
move 8 from 5 to 7
move 35 from 3 to 1
move 4 from 3 to 7
move 10 from 7 to 2
move 2 from 9 to 6
move 3 from 4 to 2
move 1 from 7 to 5
move 1 from 7 to 8
move 1 from 5 to 4
move 12 from 1 to 6
move 1 from 8 to 1
move 1 from 4 to 5
move 14 from 6 to 8
move 9 from 8 to 6
move 5 from 6 to 1
move 11 from 2 to 9
move 1 from 9 to 8
move 6 from 8 to 3
move 6 from 9 to 2
move 8 from 1 to 9
move 3 from 3 to 6
move 7 from 1 to 4
move 1 from 5 to 9
move 8 from 9 to 8
move 7 from 6 to 8
move 1 from 9 to 3
move 3 from 6 to 4
move 3 from 9 to 1
move 4 from 3 to 2
move 1 from 6 to 7
move 1 from 4 to 2
move 13 from 1 to 7
move 6 from 4 to 8
move 1 from 7 to 3
move 1 from 4 to 6
move 1 from 9 to 5
move 1 from 3 to 5
move 19 from 8 to 9
move 1 from 6 to 5
move 6 from 9 to 2
move 2 from 5 to 8
move 1 from 5 to 2
move 4 from 1 to 4
move 8 from 9 to 4
move 3 from 9 to 8
move 2 from 9 to 1
move 6 from 7 to 5
move 12 from 4 to 2
move 6 from 8 to 3
move 1 from 4 to 1
move 1 from 3 to 1
move 13 from 2 to 3
move 4 from 5 to 3
move 1 from 4 to 9
move 1 from 8 to 9
move 12 from 3 to 2
move 1 from 9 to 1
move 2 from 5 to 9
move 3 from 9 to 5
move 1 from 7 to 5
move 3 from 7 to 3
move 1 from 5 to 4
move 1 from 5 to 8
move 9 from 2 to 3
move 2 from 2 to 3
move 3 from 1 to 9
move 1 from 8 to 9
move 3 from 9 to 1
move 9 from 2 to 6
move 1 from 9 to 5
move 6 from 2 to 3
move 2 from 6 to 9
move 3 from 6 to 3
move 1 from 4 to 3
move 2 from 9 to 6
move 2 from 7 to 2
move 2 from 2 to 8
move 24 from 3 to 7
move 2 from 5 to 6
move 2 from 8 to 2
move 7 from 2 to 8
move 8 from 3 to 6
move 2 from 1 to 3
move 1 from 1 to 2
move 1 from 5 to 2
move 15 from 7 to 4
move 9 from 7 to 9
move 7 from 9 to 1
move 5 from 8 to 1
move 4 from 1 to 4
move 19 from 4 to 3
move 22 from 3 to 5
move 1 from 7 to 5
move 9 from 5 to 4
move 6 from 1 to 3
move 6 from 3 to 1
move 4 from 5 to 4
move 1 from 2 to 1
move 1 from 2 to 6
move 4 from 6 to 1
move 1 from 3 to 6
move 3 from 6 to 3
move 2 from 9 to 8
move 2 from 5 to 3
move 2 from 5 to 1
move 10 from 6 to 4
move 4 from 4 to 9
move 7 from 4 to 3
move 2 from 8 to 7
move 4 from 9 to 3
move 5 from 5 to 7
move 1 from 5 to 1
move 1 from 6 to 3
move 1 from 8 to 4
move 1 from 8 to 3
move 13 from 4 to 5
move 1 from 1 to 8
move 6 from 5 to 3
move 1 from 7 to 6
move 5 from 7 to 6
move 9 from 1 to 8
move 1 from 8 to 4
move 1 from 7 to 1
move 1 from 4 to 1
move 5 from 3 to 7
move 3 from 7 to 9
move 1 from 5 to 4
move 6 from 8 to 6
move 1 from 9 to 3
move 2 from 9 to 5
move 7 from 5 to 9
move 1 from 7 to 5
move 2 from 5 to 3
move 10 from 6 to 8
move 2 from 6 to 1
move 1 from 4 to 9
move 1 from 7 to 5
move 8 from 8 to 2
move 1 from 1 to 7
move 1 from 9 to 7
move 1 from 5 to 1
move 3 from 9 to 8
move 7 from 8 to 7
move 6 from 7 to 1
move 1 from 8 to 7
move 4 from 7 to 1
move 16 from 3 to 7
move 4 from 3 to 1
move 5 from 7 to 8
move 16 from 1 to 4
move 9 from 1 to 7
move 1 from 3 to 4
move 15 from 4 to 8
move 1 from 3 to 1
move 2 from 1 to 6
move 2 from 4 to 9
move 17 from 8 to 2
move 6 from 9 to 5
move 8 from 7 to 8
move 2 from 6 to 9
move 4 from 5 to 7
move 2 from 8 to 5
move 1 from 5 to 9
move 11 from 2 to 6
move 4 from 6 to 1
move 5 from 2 to 8
move 2 from 9 to 2
move 1 from 9 to 3
move 3 from 1 to 8
move 1 from 3 to 6
move 7 from 6 to 9
move 2 from 5 to 4
move 6 from 7 to 4
move 4 from 8 to 1
move 1 from 5 to 2
move 1 from 6 to 1
move 7 from 9 to 8
move 2 from 7 to 9
move 9 from 2 to 9
move 5 from 9 to 3
move 3 from 2 to 8
move 4 from 8 to 7
move 9 from 7 to 2
move 3 from 1 to 3
move 14 from 8 to 1
move 2 from 8 to 3
move 1 from 9 to 4
move 3 from 7 to 9
move 8 from 3 to 9
move 2 from 2 to 7
move 12 from 1 to 8
move 4 from 1 to 6
move 2 from 6 to 7
move 1 from 6 to 7
move 9 from 4 to 7
move 9 from 7 to 4
move 1 from 1 to 6
move 2 from 3 to 6
move 2 from 6 to 8
move 12 from 9 to 8
move 2 from 6 to 9
move 2 from 9 to 7
move 1 from 8 to 5
move 5 from 7 to 5
move 1 from 9 to 1
move 3 from 4 to 1
move 5 from 4 to 8
move 4 from 1 to 7
move 1 from 4 to 2
move 19 from 8 to 4
move 2 from 7 to 5
move 14 from 8 to 5
move 2 from 7 to 8
move 3 from 9 to 8
move 19 from 4 to 2
move 9 from 2 to 4
move 2 from 7 to 8
move 15 from 5 to 9
move 15 from 9 to 8
move 1 from 5 to 9
move 11 from 8 to 7
move 4 from 5 to 8
move 1 from 5 to 9
move 2 from 9 to 5
move 2 from 2 to 6
move 14 from 2 to 9
move 12 from 8 to 9
move 3 from 8 to 4
move 7 from 9 to 2
move 4 from 7 to 9
move 1 from 6 to 9
move 1 from 7 to 5
move 1 from 6 to 2
move 3 from 5 to 4
move 19 from 9 to 4
move 1 from 5 to 1
move 1 from 9 to 8
move 1 from 1 to 7
move 1 from 8 to 9
move 4 from 7 to 2
move 3 from 7 to 6
move 18 from 4 to 2
move 17 from 2 to 3
move 2 from 6 to 8
move 17 from 3 to 6
move 13 from 2 to 1
move 2 from 8 to 3
move 2 from 2 to 9
move 6 from 1 to 9
move 1 from 3 to 4
move 1 from 3 to 9
move 8 from 6 to 4
move 20 from 4 to 8
move 3 from 4 to 8
move 15 from 8 to 2
move 11 from 2 to 6
move 2 from 1 to 7
move 7 from 9 to 8
move 6 from 9 to 3
move 1 from 6 to 5";
}
