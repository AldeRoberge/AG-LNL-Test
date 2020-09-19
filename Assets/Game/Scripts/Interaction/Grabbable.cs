using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Grabbable : MonoBehaviour
    {
        [HideInInspector] public GameObject GrabbingSphere;

        private GameObject GrabbedBy;

        public void Awake()
        {
            InteractionWorld.Instance.AddGrabbable(this);

            GrabbingSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GrabbingSphere.name = "Grabbing Sphere";

            GrabbingSphere.GetComponent<SphereCollider>().isTrigger = true;
            GrabbingSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            GrabbingSphere.transform.SetParent(transform, false);
            GrabbingSphere.transform.position = new Vector3(0, 0.5f, 0);
        }

        public void SetIsHovering(bool isHovering)
        {
            if (isHovering)
                GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
            else
                GetComponentInChildren<MeshRenderer>().material.color = Color.white;
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
            this.transform.parent = GrabbedBy.transform;
        }
    }
}