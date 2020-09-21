using System.Text;
using UnityEngine;

namespace Game.Scripts.ChessBoard.Models
{
    public class Position
    {
        public byte x;
        public byte y;

        public Position(byte x, byte y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "(" + "X : " + x + ", Y : " + y + ")";
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