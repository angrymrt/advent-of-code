using System.Text;

internal class HandheldDevice
{
    public Dictionary<int, int> InterestingSignalStrengths { get; } = new Dictionary<int, int>() {
        { 20, 0 },
        { 60, 0 },
        { 100, 0 },
        { 140, 0 },
        { 180, 0 },
        { 220, 0 },
    };
    public string CrtImage
    {
        get
        {
            return crt.ToString();
        }
    }
    public int Cycle
    {
        get
        {
            return cycle;
        }
    }

    public int Execute(string instructions)
    {
        var result = 0;

        var lines = instructions.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            switch (line.Substring(0, 4))
            {
                case "noop":
                    incrementCycle();
                    break;
                case "addx":
                    add(int.Parse(line.Substring(5)));
                    break;
            }
        }

        return result;
    }

    private int cycle = 0;
    private int register = 1;
    private StringBuilder crt = new StringBuilder();

    private void updateInterestingSignalStrengths()
    {
        if (InterestingSignalStrengths.Keys.Contains(cycle))
        {
            InterestingSignalStrengths[cycle] = cycle * register;
        }
    }
    private void incrementCycle()
    {
        drawCrtPixel();
        cycle++;
        updateInterestingSignalStrengths();
    }
    private void drawCrtPixel()
    {
        var crtPosition = cycle % 40;
        if(cycle != 0 && crtPosition == 0) {
            crt.AppendLine();
        }
        if (crtPosition >= register - 1 && crtPosition <= register + 1)
        {
            crt.Append('#');
        }
        else
        {
            crt.Append('.');
        }
    }
    private void add(int value)
    {
        incrementCycle();
        incrementCycle();

        register += value;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2022, day 10");

        var device = new HandheldDevice();
        device.Execute(testInput);
        var testAnswerPart1 = device.InterestingSignalStrengths.Values.Sum();
        Console.WriteLine($"Test answer part 1: {testAnswerPart1}");
        Console.WriteLine($"Test answer part 2:");
        Console.WriteLine(device.CrtImage);

        device = new HandheldDevice();
        device.Execute(input);
        var answerPart1 = device.InterestingSignalStrengths.Values.Sum();
        Console.WriteLine($"Answer part 1: {answerPart1}");
        Console.WriteLine($"Answer part 2:");
        Console.WriteLine(device.CrtImage);
    }

    private static string testInput = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";

    private static string input = @"noop
noop
addx 5
addx 3
addx -2
noop
addx 5
addx 4
noop
addx 3
noop
addx 2
addx -17
addx 18
addx 3
addx 1
noop
addx 5
noop
addx 1
addx 2
addx 5
addx -40
noop
addx 5
addx 2
addx 3
noop
addx 2
addx 3
addx -2
addx 2
addx 2
noop
addx 3
addx 5
addx 2
addx 3
addx -2
addx 2
addx -24
addx 31
addx 2
addx -33
addx -6
addx 5
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx -1
addx 6
noop
noop
addx 1
addx 4
noop
noop
addx -15
addx 20
noop
addx -23
addx 27
noop
addx -35
addx 1
noop
noop
addx 5
addx 11
addx -10
addx 4
addx 1
noop
addx 2
addx 2
noop
addx 3
noop
addx 3
addx 2
noop
addx 3
addx 2
addx 11
addx -4
addx 2
addx -38
addx -1
addx 2
noop
addx 3
addx 5
addx 2
addx -7
addx 8
addx 2
addx 2
noop
addx 3
addx 5
addx 2
addx -25
addx 26
addx 2
addx 8
addx -1
addx 2
addx -2
addx -37
addx 5
addx 3
addx -1
addx 5
noop
addx 22
addx -21
addx 2
addx 5
addx 2
addx 13
addx -12
addx 4
noop
noop
addx 5
addx 1
noop
noop
addx 2
noop
addx 3
noop
noop";
}