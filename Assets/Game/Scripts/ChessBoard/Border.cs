using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Border : MonoBehaviour
    {
        public void Start()
        {
            this.GetComponent<Renderer>().material = ChessBoardAssets.Instance.BorderMaterial;
        }
    }
}