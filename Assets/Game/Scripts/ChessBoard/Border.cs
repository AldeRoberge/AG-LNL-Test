using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Border : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<Renderer>().material = ChessBoardAssets.Instance.BorderMaterial;
        }
    }
}