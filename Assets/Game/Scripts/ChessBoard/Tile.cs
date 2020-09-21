using Game.Scripts.ChessBoard.Models;
using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Tile : MonoBehaviour
    {
        public Position Position;
        private TargetPlaneHandler TargetPlaneHandler;

        public bool IsValid { get; set; }

        public void Awake()
        {
            InteractionWorld.Instance.AddTile(this);
        }

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

            piece.transform.localScale = new Vector3(1, 1, 1);
        }

        public void SetValid(bool isValid)
        {
            if (isValid)
            {
                IsValid = isValid;
                this.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                IsValid = isValid;
                this.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}