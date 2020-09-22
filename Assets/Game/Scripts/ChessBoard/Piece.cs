using System;
using DefaultNamespace;
using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Piece : MonoBehaviour
    {
        private Color CachedColor;

        public void Start()
        {
            CachedColor = GetComponentInChildren<MeshRenderer>().material.color;
            AddHoverListeners();
        }

        private void AddHoverListeners()
        {
            Grabbable g = GetComponent<Grabbable>();
            g.OnHoverStart.AddListener(() => { GetComponentInChildren<MeshRenderer>().material.color = Color.yellow; });
            g.OnHoverStart.AddListener(() => { GetComponentInChildren<MeshRenderer>().material.color = CachedColor; });
        }
    }
}