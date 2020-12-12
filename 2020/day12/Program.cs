using System;
using System.Linq;

namespace day12
{
    
    class Instruction
    {
        public Instruction(string raw)
        {
            Direction = raw.Substring(0,1);
            Distance = int.Parse(raw.Substring(1));
        }

        public string Direction { get; }
        public int Distance { get; }
    }
    class Ferry {
        public int DistanceNorth = 0;
        public int DistanceEast = 0;
        public Directions Facing = Directions.E;
        public Ferry Waypoint; 

        public Ferry()
        {
            Waypoint = new Ferry(10, 1);
        }
        public Ferry(int east, int north)
        {
            DistanceEast = east;
            DistanceNorth = north;
        }

        public int AbsoluteDistanceFromStart(){
            var northSouth = DistanceNorth < 0 ? 0 - DistanceNorth : DistanceNorth;
            var eastWest = DistanceEast < 0 ? 0 - DistanceEast : DistanceEast;
            return northSouth + eastWest;
        }
        public string Execute(Instruction instruction) {
            switch(instruction.Direction) {
                case "R": {
                    var modulo = (((int)Facing + (instruction.Distance / 90)) % 4);
                    Facing = (Directions)modulo;
                    return $"Turning right {instruction.Distance} degrees, now facing: {Facing}";
                }
                case "L": {
                    var modulo = (((int)Enum.Parse<DirectionsLeftTurn>(Facing.ToString()) + (instruction.Distance / 90)) % 4);
                    Facing = Enum.Parse<Directions>(((DirectionsLeftTurn)modulo).ToString());
                    return $"Turning left {instruction.Distance} degrees, now facing: {Facing}";
                }
                case "F":
                    return Execute(new Instruction($"{Facing.ToString()}{instruction.Distance}"));
                case "E":
                    DistanceEast += instruction.Distance;
                    break;
                case "W":
                    DistanceEast -= instruction.Distance;
                    break;
                case "N":
                    DistanceNorth += instruction.Distance;
                    break;
                case "S":
                    DistanceNorth -= instruction.Distance;
                    break;
            }
            return $"Heading {instruction.Direction} for {instruction.Distance} to {ToString()}";
        }
        public string ExecutePart2(Instruction instruction) {
            switch(instruction.Direction) {
                case "R": {
                    var currentFacing = Facing;
                    var inverseDistance = 0 - instruction.Distance;
                    var angle = inverseDistance * (Math.PI / 180); // Convert to radians
                    var rotatedX = Math.Cos(angle) * (Waypoint.DistanceEast - DistanceEast) - Math.Sin(angle) * (Waypoint.DistanceNorth - DistanceNorth) + DistanceEast;
                    var rotatedY = Math.Sin(angle) * (Waypoint.DistanceEast - DistanceEast) + Math.Cos(angle) * (Waypoint.DistanceNorth - DistanceNorth) + DistanceNorth;
                    Waypoint.DistanceEast = (int)Math.Round(rotatedX);
                    Waypoint.DistanceNorth = (int)Math.Round(rotatedY);
                    return $"Rotating right {instruction.Distance} degrees, new waypoint position: {Waypoint.ToString()}";
                }
                case "L": {
                    var currentFacing = Facing;
                    var angle = instruction.Distance * (Math.PI / 180); // Convert to radians
                    var rotatedX = Math.Cos(angle) * (Waypoint.DistanceEast - DistanceEast) - Math.Sin(angle) * (Waypoint.DistanceNorth - DistanceNorth) + DistanceEast;
                    var rotatedY = Math.Sin(angle) * (Waypoint.DistanceEast - DistanceEast) + Math.Cos(angle) * (Waypoint.DistanceNorth - DistanceNorth) + DistanceNorth;
                    Waypoint.DistanceEast = (int)Math.Round(rotatedX);
                    Waypoint.DistanceNorth = (int)Math.Round(rotatedY);
                    return $"Rotating left {instruction.Distance} degrees, new waypoint position: {Waypoint.ToString()}";
                }
                case "F":
                    // move x times towards waypoint..
                    var moveEast = (Waypoint.DistanceEast - DistanceEast) * instruction.Distance;
                    var moveNorth = (Waypoint.DistanceNorth - DistanceNorth) * instruction.Distance;
                    var currentPos = ToString();
                    var waypointPos = Waypoint.ToString();
                    DistanceEast += moveEast;
                    Waypoint.DistanceEast += moveEast;
                    DistanceNorth += moveNorth;
                    Waypoint.DistanceNorth += moveNorth;
                    return $"Heading {instruction.Distance} times from {currentPos} to {waypointPos} ending up in {ToString()}";
                case "E":
                case "W":
                case "N":
                case "S":
                    Waypoint.Execute(instruction);
                    break;
            }
            return $"Moving waypoint {instruction.Direction} for {instruction.Distance} to {Waypoint.ToString()}";
        }
        public override string ToString(){
            var northSouthUnit = DistanceNorth < 0 ? "S" : "N";
            var northSouthVal = DistanceNorth < 0 ? 0 - DistanceNorth : DistanceNorth;
            var eastWestUnit = DistanceEast < 0 ? "W" : "E";
            var eastWestVal = DistanceEast < 0 ? 0 - DistanceEast : DistanceEast;
            return $"{eastWestUnit}{eastWestVal}, {northSouthUnit}{northSouthVal}";
        }
    }
    public enum Directions {
        E,S,W,N
    }
    public enum DirectionsLeftTurn {
        E,N,W,S
    }

    class Program
    {
        static void Main(string[] args)
        {
            var instructions = testInput.Split(Environment.NewLine)
                                    .Select(x => new Instruction(x));

            var ferry = new Ferry();
            // foreach(var instruction in instructions){
            //     Console.WriteLine(ferry.Execute(instruction));
            // }
            // Console.WriteLine($"Answer for day 12 test input, part 1: {ferry.AbsoluteDistanceFromStart()}");

            // ferry = new Ferry();
            // foreach(var instruction in instructions){
            //     Console.WriteLine(ferry.ExecutePart2(instruction));
            // }
            // Console.WriteLine($"Answer for day 12 test input, part 2: {ferry.AbsoluteDistanceFromStart()}");

            instructions = input.Split(Environment.NewLine)
                                .Select(x => new Instruction(x));
            // ferry = new Ferry();
            // foreach(var instruction in instructions){
            //     Console.WriteLine(ferry.Execute(instruction));
            // }
            // Console.WriteLine($"Answer for day 12, part 1: {ferry.AbsoluteDistanceFromStart()}");
            // Console.WriteLine($"Current position: {ferry.ToString()}");

            // ferry = new Ferry();
            foreach(var instruction in instructions){
                Console.WriteLine(ferry.ExecutePart2(instruction));
            }
            Console.WriteLine($"Answer for day 12, part 2: {ferry.AbsoluteDistanceFromStart()}");
            Console.WriteLine($"Current position: {ferry.ToString()}");
       }

        private static string testInput = @"F10
N3
F7
R90
F11";
        private static string input = @"N3
L90
F63
W5
F46
E3
F22
N2
R90
F68
E4
W3
R90
S3
W4
R180
E1
S5
F90
N4
E3
N1
R90
F74
R90
E2
R90
W1
S3
W4
F5
S1
E5
S1
E4
R90
E5
L90
E4
R90
E2
F57
N1
L90
F59
R90
N1
W3
S2
L90
N3
E1
F56
L180
S3
R90
F88
E3
F59
W1
N2
F52
W4
F69
W2
F10
W1
R180
W1
R90
F14
L90
W1
S5
L90
S3
R90
E3
F35
R90
E3
S3
F45
E2
R90
F86
E1
E4
F35
L180
S1
L90
N2
F71
L180
W3
S4
R90
N5
F93
W4
F74
L180
E2
R180
F11
S5
F28
S3
F93
W2
N4
F26
R90
S4
L90
N1
L90
E2
L90
F3
E4
F43
R90
W4
R90
E3
S1
R180
L90
F62
L90
E5
R90
W3
L180
F40
F20
N2
L270
E1
F14
W3
S5
R90
F3
S2
L90
W5
L270
W1
R90
F11
R90
E3
N1
E3
F19
S5
L180
N4
E2
R180
E5
S2
W4
S3
W1
F4
L90
S2
W4
S5
F21
L180
W4
S3
L90
S4
L90
E1
F28
L180
S3
E2
N3
L180
W3
L90
F99
S2
F63
E2
N3
R90
E3
L90
E5
L90
N4
F39
R180
S3
R90
N3
F7
E3
S2
E2
F98
S1
F87
E1
S3
F49
N1
W2
F4
L270
F91
L90
E1
S4
R180
F43
S3
E3
R90
F46
W2
R90
W5
F13
R180
F52
N4
F28
N3
R90
E5
S3
F82
R90
W3
L90
F33
S5
R90
R90
S5
F24
R90
N4
F89
W1
S4
F80
W3
L270
F11
L90
W2
N3
F18
R90
W2
R90
E1
R270
N3
R180
S4
F36
S3
L90
N2
L90
N2
E1
F48
E5
L180
S3
F81
E4
L90
W3
F31
E5
R90
F66
S4
W3
L90
E3
N4
F85
L90
F58
E5
L90
S1
W3
F79
S4
F60
N2
F42
S3
W3
R90
E1
N1
L90
F15
E4
F98
L90
R90
S4
E1
F19
E2
S4
R90
W2
L180
N3
E2
S3
F34
S4
S4
L180
S1
R90
S4
S1
L90
E3
F28
R90
W1
N2
E5
F48
E4
S1
W2
F95
W2
N2
L90
E2
L90
W3
S2
L270
W4
L90
N4
R90
E4
R270
W4
F6
W2
N1
E1
F19
W2
N1
F54
W2
L90
S1
L90
F80
E1
S5
E5
F80
R90
L270
E4
F93
N4
E5
S1
E1
R90
F63
N3
R90
E1
N2
L90
W5
R90
R270
N1
E4
L180
E4
F19
L90
F27
W2
S2
W5
S1
F54
S4
R90
F85
W2
F13
R90
F73
S5
E2
S2
F12
W5
F23
N1
E1
F38
N2
W2
N3
E2
L270
F7
L90
S3
L90
S3
F86
E5
R90
E1
F52
L180
S4
L180
W4
F41
R90
E3
F70
R270
N3
F32
S2
E5
R180
F20
W3
F54
E2
F34
F61
S5
W1
L90
S5
N5
W2
R180
W2
L90
E5
S4
L90
S4
L180
F84
S1
W1
L90
F92
F46
N1
F22
F24
L90
N5
W4
R270
F79
N1
W1
F68
R90
W5
R180
N5
L90
L180
S1
W4
N1
L180
S1
N4
E4
R90
E1
E4
F58
S4
E5
F49
N1
E2
S4
L90
W2
F67
E2
N5
W1
L90
E5
F82
N5
F91
W5
R90
F17
W5
S2
R90
N2
R90
N5
E4
L90
N1
F26
N3
E3
F19
L270
R90
E3
F21
L180
S4
F50
S4
W2
F56
F49
N2
E3
R180
E4
F5
F17
E2
R90
N3
F96
L180
E4
F64
W5
R90
W5
S5
F92
E5
F10
N1
W1
F94
R90
W4
F22
S1
W4
F38
W1
F17
E3
L90
F3
S1
L90
F27
W4
F31
S5
W4
N2
E5
F44
W2
E4
F54
L180
E5
L90
N1
E5
N4
L180
L270
W3
F80
S2
F49
E4
F46
E2
E5
L270
F12
F63
L90
N2
E5
N3
F85
R270
S3
F71
N4
E5
F36
N5
F23
L90
N2
E3
F93
S5
F1
S2
F29
L90
F17
R180
S4
R90
E2
S3
W5
R90
S3
R90
W4
F62
L180
S4
L90
N2
F46
N3
R180
E1
R90
F73
S5
F12
L180
F47
L90
F79
N4
R270
W3
N1
W1
N3
F63
S2
F50
R90
F30
N3
F7
N4
L90
S4
N1
E5
S5
F9
L90
L90
F7
N1
R90
F52
E3
L90
N3
F50
L90
F83
E3
F74
L90
N1
L90
F4
N1
F28
E4
F9
E4
S2
W4
L270
S1
W4
F23
E1
F52
E1
L180
E2
N5
L90
W5
L90
S1
E3
R90
E4
L90
S1
W2
N4
W1
S4
E2
L90
E5
S2
L180
F91
N5
W4
N5
F14
S5
R90
S5
L90
F78
N2
W3
R90
F17
N5
W1
F53
W2
F33
R90
E2
F15
L90
E5
F77
L90
S1
F33";
    }
}
