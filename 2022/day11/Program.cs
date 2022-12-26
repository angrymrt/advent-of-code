internal class MonkeySimulation
{
    public List<Monkey> Monkeys { get; } = new List<Monkey>();

    public long Execute(string input, int rounds = 20, bool worryRelief = true, bool debug = false)
    {
        var monkeyInputs = input.Split(Environment.NewLine + Environment.NewLine);
        foreach (var monkeyInput in monkeyInputs)
        {
            Monkeys.Add(new Monkey(monkeyInput));
        }

        var magicModulo = Monkeys
            .Select(x => x.TestOperand)
            .Aggregate((a, b) => a * b);

        for (int i = 0; i < rounds; i++)
        {
            foreach (var monkey in Monkeys)
            {
                monkey.TakeTurn(Monkeys[monkey.TestTrueIndex], Monkeys[monkey.TestFalseIndex], worryRelief, magicModulo);
            }

            printMonkeyInspections(i + 1, debug);
        }

        return Monkeys
            .OrderByDescending(x => x.Inspections)
            .Take(2)
            .Select(x => (long)x.Inspections)
            .Aggregate((a, b) => a * b);
    }

    private void printMonkeyInspections(int round, bool debug)
    {
        if (debug && (round % 1000 == 0 || round == 1 || round == 20))
        {
            Console.WriteLine($"== After round {round} ==");
            foreach (var monkey in Monkeys)
            {
                Console.WriteLine($"Monkey x inspected items {monkey.Inspections} times.");
            }
            Console.WriteLine();
        }
    }
}
internal class Monkey
{
    public int Inspections { get; private set; } = 0;
    public Queue<long> Items { get; } = new Queue<long>();
    public char OperationOperator { get; private set; }
    public string OperationOperand { get; private set; }
    public int TestOperand { get; private set; }
    public int TestTrueIndex { get; private set; }
    public int TestFalseIndex { get; private set; }

    public void TakeTurn(Monkey monkeyTrue, Monkey monkeyFalse, bool worryRelief, int magicModulo)
    {
        while (Items.Count > 0)
        {
            Inspections++;
            var worryLevel = Items.Dequeue();
            var operand = worryLevel;
            if (OperationOperand != "old")
            {
                operand = int.Parse(OperationOperand);
            }

            if (OperationOperator == '*')
            {
                worryLevel *= operand;
            }
            else
            {
                worryLevel += operand;
            }
            if (worryRelief)
            {
                worryLevel /= 3;
            }
            else
            {
                worryLevel = manageWorryLevel(worryLevel, magicModulo);
            }
            if (worryLevel % TestOperand == 0)
            {
                monkeyTrue.Items.Enqueue(worryLevel);
            }
            else
            {
                monkeyFalse.Items.Enqueue(worryLevel);
            }
        }
    }

    private long manageWorryLevel(long worryLevel, int magicModulo)
    {
        if (worryLevel <= magicModulo)
        {
            return worryLevel;
        }
        var newValue = worryLevel % magicModulo;
        if (newValue == 0)
        {
            return magicModulo;
        }
        return newValue;
    }

    public Monkey(string input)
    {
        /* Example input:
         * Monkey 0:
         *   Starting items: 79, 98
         *   Operation: new = old * 19
         *   Test: divisible by 23
         *     If true: throw to monkey 2
         *     If false: throw to monkey 3
         */
        var lines = input.Split(Environment.NewLine);
        var startingItems = lines[1]
            .Substring(17)
            .Split(',')
            .Select(x => long.Parse(x.Substring(1)));
        foreach (var item in startingItems)
        {
            Items.Enqueue(item);
        }
        OperationOperator = lines[2][23];
        OperationOperand = lines[2].Substring(25);
        TestOperand = int.Parse(lines[3].Substring(21));
        TestTrueIndex = int.Parse(lines[4].Substring(29));
        TestFalseIndex = int.Parse(lines[5].Substring(30));
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2022, day 11");

        var sim = new MonkeySimulation();
        var answer = sim.Execute(testInput);
        Console.WriteLine($"Test answer part 1: {answer}");

        sim = new MonkeySimulation();
        answer = sim.Execute(input);
        Console.WriteLine($"Answer part 1: {answer}");

        sim = new MonkeySimulation();
        answer = sim.Execute(testInput, 10_000, false, true);
        Console.WriteLine($"Test answer part 2: {answer}");

        sim = new MonkeySimulation();
        answer = sim.Execute(input, 10_000, false);
        Console.WriteLine($"Answer part 2: {answer}");
    }

    private static string testInput = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

    private static string input = @"Monkey 0:
  Starting items: 98, 89, 52
  Operation: new = old * 2
  Test: divisible by 5
    If true: throw to monkey 6
    If false: throw to monkey 1

Monkey 1:
  Starting items: 57, 95, 80, 92, 57, 78
  Operation: new = old * 13
  Test: divisible by 2
    If true: throw to monkey 2
    If false: throw to monkey 6

Monkey 2:
  Starting items: 82, 74, 97, 75, 51, 92, 83
  Operation: new = old + 5
  Test: divisible by 19
    If true: throw to monkey 7
    If false: throw to monkey 5

Monkey 3:
  Starting items: 97, 88, 51, 68, 76
  Operation: new = old + 6
  Test: divisible by 7
    If true: throw to monkey 0
    If false: throw to monkey 4

Monkey 4:
  Starting items: 63
  Operation: new = old + 1
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1

Monkey 5:
  Starting items: 94, 91, 51, 63
  Operation: new = old + 4
  Test: divisible by 13
    If true: throw to monkey 4
    If false: throw to monkey 3

Monkey 6:
  Starting items: 61, 54, 94, 71, 74, 68, 98, 83
  Operation: new = old + 2
  Test: divisible by 3
    If true: throw to monkey 2
    If false: throw to monkey 7

Monkey 7:
  Starting items: 90, 56
  Operation: new = old * old
  Test: divisible by 11
    If true: throw to monkey 3
    If false: throw to monkey 5";
}