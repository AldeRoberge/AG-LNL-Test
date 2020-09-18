using Game.Scripts.ChessBoard.Models;
using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Tile : MonoBehaviour
    {
        public Position Position;

        public void Start()
        {
            GameObject m = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            m.GetComponent<SphereCollider>().isTrigger = true;

            BoxCollider b = GetComponent<BoxCollider>();
            m.transform.position = b.center;
        }

        public void SetPosition(Position position)
        {
            this.Position = position;
        }

        public void SetPiece(GameObject createPieceAt)
        {
            createPieceAt.transform.parent = this.transform;
        }
    }
}