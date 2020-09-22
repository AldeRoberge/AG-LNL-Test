using System;
using Game.Scripts;
using Game.Scripts.ChessBoard;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Grabbable : MonoBehaviour
    {
        [HideInInspector] public GameObject GrabbingSphere;

        public Tile Tile;

        private GameObject GrabbedBy;

        public UnityEvent OnHoverStart = new UnityEvent();
        public UnityEvent OnHoverStop = new UnityEvent();

        public void Awake()
        {
            InteractionWorld.Instance.AddGrabbable(this);

            GrabbingSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GrabbingSphere.name = "Grabbing Sphere";

            GrabbingSphere.GetComponent<SphereCollider>().isTrigger = true;
            GrabbingSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            GrabbingSphere.transform.SetParent(transform, false);
            GrabbingSphere.transform.position = new Vector3(0, 0.5f, 0);

            if (!Config.Debug)
            {
                GrabbingSphere.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        public void SetCurrentTile(Tile tile)
        {
            Tile = tile;
        }

        public void SetIsHovered(bool isHovered)
        {
            if (isHovered)
                OnHoverStart.Invoke();
            else
                OnHoverStop.Invoke();
        }

        public void Update()
        {
            if (GrabbedBy != null)
            {
                gameObject.transform.position = GrabbedBy.transform.position;
            }
        }

        public void GrabbingStopped()
        {
            GrabbedBy = null;
        }

        public void GrabbingStart(GameObject GrabbedBy)
        {
            this.GrabbedBy = GrabbedBy;
        }
    }
}