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
            TargetPlaneHandler = gameObject.AddComponent<TargetPlaneHandler>();
            Position = position;
        }

        public void SetPiece(GameObject piece)
        {
            piece.transform.SetParent(transform, false);
            piece.transform.position += new Vector3(0, 4, 0);
        }
    }
}