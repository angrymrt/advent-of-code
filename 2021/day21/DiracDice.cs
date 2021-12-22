using System;
using System.Collections.Generic;
using System.Linq;

namespace day21
{
    public class DiracDice
    {
        public QuantumDie Die { get; }
        public PossibleGameState StartState { get; }
        public int Turn { get; private set; }
        public PossibleGameState[] OpenGames { get; private set; }
        public List<PossibleGameState> ClosedGames { get; }

        public DiracDice(int startingPositionPlayer1, int startingPositionPlayer2)
        {
            Die = new QuantumDie();
            StartState = new PossibleGameState(startingPositionPlayer1, startingPositionPlayer2);
            Turn = 0;
            OpenGames = new PossibleGameState[] { StartState };
            ClosedGames = new List<PossibleGameState>();
        }

        public bool ExecutePlayerTurn()
        {
            var currentStates = OpenGames;
            var newStates = new List<PossibleGameState>();
            Console.WriteLine($"Turn {Turn}, OpenGames {OpenGames.Sum(x => x.Dimensions)}, ClosedGames {ClosedGames.Sum(x => x.Dimensions)}");
            foreach (var stateGroup in currentStates.GroupBy(x => x.StateHash))
            {
                var state = stateGroup.First();
                state.Dimensions = stateGroup.Sum(x => x.Dimensions);
                foreach (var possibleOutcome in Die.PossibleOutcomes3Roles)
                {
                    var newState = new PossibleGameState(state, Turn % 2, possibleOutcome.Key, possibleOutcome.Value);
                    if (newState.HasWinner)
                    {
                        ClosedGames.Add(newState);
                    }
                    else
                    {
                        newStates.Add(newState);
                    }
                }
            }
            Turn++;
            OpenGames = newStates.ToArray();
            return newStates.Any();
        }
    }
}