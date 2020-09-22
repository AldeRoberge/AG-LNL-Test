using System;
using Game.Scripts;
using Game.Scripts.ChessBoard;
using UnityEngine;

namespace DefaultNamespace
{
    public class Grabbable : MonoBehaviour
    {
        [HideInInspector] public GameObject GrabbingSphere;

        public Tile Tile;

        private GameObject GrabbedBy;


        private Color cachedColor;

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
            
            cachedColor = GetComponentInChildren<MeshRenderer>().material.color;
        }

        public void SetCurrentTile(Tile tile)
        {
            this.Tile = tile;
        }

        public void SetIsHovering(bool isHovering)
        {
            if (isHovering)
                GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
            else
                GetComponentInChildren<MeshRenderer>().material.color = cachedColor;
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
            this.transform.parent = null;
        }

        public void GrabbingStart(GameObject GrabbedBy)
        {
            this.GrabbedBy = GrabbedBy;
            this.transform.parent = GrabbedBy.transform;
        }
    }
}