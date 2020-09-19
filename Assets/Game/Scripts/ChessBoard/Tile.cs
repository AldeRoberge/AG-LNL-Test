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

        public GameObject piece;

        public void SetPiece(GameObject piece)
        {
            piece.transform.SetParent(transform, false);

            this.piece = piece;

            // Place piece
            GameObject children = piece.transform.GetChild(0).gameObject;

            float boxHalfSize = ((gameObject.GetComponent<BoxCollider>().size.y) / 2);
            float pieceHalfSize = ((children.GetComponent<BoxCollider>().size.z * children.GetComponent<Transform>().localScale.z) / 2);

            if (Config.Debug) Debug.Log("Half size of box is " + boxHalfSize + " and piece " + pieceHalfSize);

            // Move to accurate position
            piece.transform.localPosition = new Vector3(0,
                boxHalfSize + pieceHalfSize,
                0);
        }
    }


}