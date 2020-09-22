using System;
using DefaultNamespace;
using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class Piece : DestroyableMonoBehaviour
    {
        private Color CachedColor;

        private Grabbable Grabbable;

        public void Start()
        {
            CachedColor = GetComponentInChildren<MeshRenderer>().material.color;
            AddHoverListeners();
        }

        private void AddHoverListeners()
        {
            Grabbable = GetComponentInParent<Grabbable>();
            Grabbable.OnHoverStart.AddListener(() =>
            {
                
                GetComponent<MeshRenderer>().material.color = Color.yellow;
            });
            Grabbable.OnHoverStop.AddListener(() =>
            {
                GetComponent<MeshRenderer>().material.color = CachedColor;
            });
        }

        public override void Destroy()
        {
            InteractionWorld.Instance.RemoveGrabbable(Grabbable);
            Destroy(gameObject);
        }
    }
}