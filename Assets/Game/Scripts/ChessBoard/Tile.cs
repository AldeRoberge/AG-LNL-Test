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
        }

        public void Update()
        {
            if (piece != null)
            {
                GameObject children = piece.transform.GetChild(0).gameObject;

                Debug.Log(children.GetComponent<Transform>().localScale.z);

                float boxHalfSize = ((gameObject.GetComponent<BoxCollider>().size.y * gameObject.transform.localScale.y) / 2) - gameObject.GetComponent<BoxCollider>().center.y;
                float pieceHalfSize = ((children.GetComponent<BoxCollider>().size.z * children.GetComponent<Transform>().localScale.z) / 2 ) - children.GetComponent<BoxCollider>().center.z;

                Debug.Log("Half size of box is " + boxHalfSize + " and piece " + pieceHalfSize);
                Debug.Log("Box full size : " + (boxHalfSize * 2));
                Debug.Log("Total : " + (boxHalfSize + pieceHalfSize));

                // Move to accurate position
                piece.transform.localPosition = new Vector3(0,
                    boxHalfSize + pieceHalfSize,
                    0);
            }
        }
    }
}