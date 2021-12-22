using System;
using System.Linq;

namespace day21
{
    public class PossibleGameState
    {
        public int[] PlayerPositions { get; }
        public int[] PlayerScores { get; }
        public long Dimensions { get; set; }
        public bool HasWinner => PlayerScores.Any(x => x >= 21);
        public int WinningPlayerIndex => PlayerScores[0] >= 21 ? 0 : 1;

        public string StateHash => $"{PlayerPositions[0]},{PlayerPositions[1]},{PlayerScores[0]},{PlayerScores[1]}";

        public PossibleGameState(int player1Position, int player2Position)
        {
            PlayerPositions = new int[]
            {
                player1Position,
                player2Position,
            };
            PlayerScores = new int[] { 0, 0 };
            Dimensions = 1;
        }
        public PossibleGameState(PossibleGameState fromState, int playerIndex, int spacesToMove, int playerCount)
        {
            PlayerPositions = new int[]
            {
                fromState.PlayerPositions[0],
                fromState.PlayerPositions[1],
            };
            PlayerPositions[playerIndex] = Board.MoveSpaces(PlayerPositions[playerIndex], spacesToMove);
            PlayerScores = new int[] 
            {
                fromState.PlayerScores[0],
                fromState.PlayerScores[1],
            };
            PlayerScores[playerIndex] += PlayerPositions[playerIndex];
            Dimensions = fromState.Dimensions * playerCount;
        }
    }
}