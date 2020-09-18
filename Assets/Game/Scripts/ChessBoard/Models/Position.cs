using System.Text;
using UnityEngine;

namespace Game.Scripts.ChessBoard.Models
{
    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static class PositionUtils
    {
        public static readonly char[] s = new[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'};

        public static string ToChessCoreString(this Position position)
        {
            int x = position.x;
            int y = position.y;

            if (x > ChessBoardConstants.Size)
                Debug.LogError("[PositionUtils] X should not be larger than chess board size.");

            if (y > ChessBoardConstants.Size)
                Debug.LogError("[PositionUtils] Y should not be larger than chess board size.");

            return (s[x] + y).ToString();
        }
    }
}