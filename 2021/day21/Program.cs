using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day21
{
    public class Player
    {
        public int Position { get; private set; }
        public IDie Die { get; }
        public int Score { get; private set; }

        public Player(int startingPosition, IDie die)
        {
            Position = startingPosition;
            Die = die;
        }

        public int TakeTurn()
        {
            var spacesToMove = 0;
            for (int i = 0; i < 3; i++)
            {
                spacesToMove += Die.Roll();
            }
            Position = Board.MoveSpaces(Position, spacesToMove);
            Score += Position;
            return Score;
        }
    }

    public class Board
    {
        public static int Spaces { get; } = 10;

        public static int MoveSpaces(int position, int spacesToMove)
        {
            var newPosition = position + spacesToMove % Spaces;
            var result = newPosition <= Spaces ? newPosition : newPosition - Spaces;
            return result;
        }
    }

    public class DetirministicDie : IDie
    {
        public int Sides { get; }
        public int NextFaceUp { get; private set; }
        public int RollCount { get; private set; }

        public DetirministicDie(int sides = 100)
        {
            Sides = sides;
            NextFaceUp = 1;
            RollCount = 0;
        }

        public int Roll()
        {
            RollCount++;
            var result = NextFaceUp;
            NextFaceUp = NextFaceUp == Sides ? 1 : NextFaceUp + 1;
            return result;
        }
    }
    public class QuantumDie : IDie
    {
        private Random _random;

        public int Sides { get; }
        public Dictionary<int, int> PossibleOutcomes3Roles { get; }

        public QuantumDie(int sides = 3)
        {
            Sides = sides;
            PossibleOutcomes3Roles = new Dictionary<int, int>() {
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 },
                { 7, 0 },
                { 8, 0 },
                { 9, 0 },
            };
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        PossibleOutcomes3Roles[i + j + k]++;
                    }
                }
            }
            _random = new Random();
        }

        public int Roll()
        {
            return _random.Next(1, 4);
        }
    }
    public interface IDie {
        int Roll();
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 21!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var die = new DetirministicDie();
            var input = (useTestInput ? new int[] { 4, 8 } : new int[] { 7, 1 });
            var players = input
                .Select(x => new Player(x, die))
                .ToArray();


            // Start part 1.
            sw.Start();
            while (true)
            {
                var playerReached1000 = false;
                foreach (var player in players)
                {
                    if (player.TakeTurn() >= 1000)
                    {
                        playerReached1000 = true;
                        break;
                    }
                }
                if (playerReached1000)
                {
                    break;
                }
            }
            var loser = players.OrderBy(x => x.Score).First();
            answer = loser.Score * die.RollCount;
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            var game = new DiracDice(input[0], input[1]);
            while(game.ExecutePlayerTurn()) { }
            var player1Wins = game.ClosedGames
                .Where(x => x.WinningPlayerIndex == 0)
                .Sum(x => x.Dimensions);
            var player2Wins = game.ClosedGames
                .Where(x => x.WinningPlayerIndex == 1)
                .Sum(x => x.Dimensions);
            answer = player1Wins > player2Wins ? player1Wins : player2Wins;
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"Player 1 starting position: 4
Player 2 starting position: 8";
        public static string input = @"Player 1 starting position: 7
Player 2 starting position: 1";
    }
}
