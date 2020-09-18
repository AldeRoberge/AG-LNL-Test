using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Grabbable : MonoBehaviour
    {
        [HideInInspector] public GameObject GrabbableObject;
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
            
            SetGrabbingSphereOnTop();
        }

        public void SetGrabbableObject(GameObject grabbableObject)
        {
            GrabbableObject = grabbableObject;
        }

        private void SetGrabbingSphereOnTop()
        {
           /* BoxCollider boxCollider = this.gameObject.GetComponentInChildren<BoxCollider>();

            // Sets the position 
            GrabbingSphere.transform.position += new Vector3(0,
                (((boxCollider.size.y) *
                    GrabbableObject.transform.lossyScale.y + 1)) * 1.5f, 0);*/
        }

        public void SetIsHovering(bool isHovering)
        {
            if (isHovering)
                GrabbableObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            else
                GrabbableObject.GetComponent<MeshRenderer>().material.color = Color.white;
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
            GrabbableObject.GetComponent<Rigidbody>().isKinematic = false;
            GrabbableObject.GetComponent<Rigidbody>().detectCollisions = true;
        }

        public void GrabbingStart(GameObject GrabbedBy)
        {
            this.GrabbedBy = GrabbedBy;
            GrabbableObject.GetComponent<Rigidbody>().isKinematic = true;
            GrabbableObject.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }
}