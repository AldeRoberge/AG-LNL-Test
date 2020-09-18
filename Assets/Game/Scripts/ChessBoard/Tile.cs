using Game.Scripts.ChessBoard.Models;
using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Tile : MonoBehaviour
    {
        public Position Position;
        private TargetPlaneHandler TargetPlaneHandler;

        public void SetPosition(Position position)
        {
            Position = position;
        }

        public void SetPiece(GameObject piece)
        {
            piece.transform.parent = transform;
            piece.transform.position += new Vector3(0, 2, 0);
        }
    }
}