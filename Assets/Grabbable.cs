using UnityEngine;

namespace DefaultNamespace
{
    public class Grabbable : MonoBehaviour
    {
        private GameObject GrabbableObject;
        private GameObject GrabbingSphere;

        public void Awake()
        {
            GrabbingSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            GrabbingSphere.GetComponent<SphereCollider>().isTrigger = true;
            GrabbingSphere.transform.localScale = new Vector3(1f, 1f, 1f);
            GrabbingSphere.transform.SetParent(transform, false);
        }

        public void SetGrabbableObject(GameObject grabbableObject)
        {
            GrabbableObject = grabbableObject;
            SetGrabbingSphereOnTop();
        }

        private void SetGrabbingSphereOnTop()
        {
            BoxCollider boxCollider = GrabbableObject.GetComponent<BoxCollider>();
            
            
            // Sets the position 
            GrabbingSphere.transform.position += new Vector3(0, 
                                                                (((boxCollider.size.y) *
                                                                 GrabbableObject.transform.lossyScale.y + 1)) * 1.5f, 0);
            
            
            
        }
    }
}