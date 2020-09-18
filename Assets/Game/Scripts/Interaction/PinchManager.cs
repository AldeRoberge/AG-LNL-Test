using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Utils;

public class PinchManager : MonoBehaviour
{
    private PinchDetectorInitializer p;

    public Grabbable currentlyHovering;


    public Grabbable currentlyGrabbing;

    // Start is called before the first frame update
    void Start()
    {
        p = gameObject.AddComponent<PinchDetectorInitializer>();

        p.RightHandPinchDetector.OnPinchStart.AddListener(() => { Grab(p.RightHandPinchDetector.FingerTipObj); });
        p.RightHandPinchDetector.OnPinchEnd.AddListener(() => { Ungrab(); });
    }

    private void Ungrab()
    {
        if (currentlyGrabbing == null)
        {
            return;
        }

        currentlyGrabbing.GrabbingStopped();
    }

    private void Grab(GameObject fingerTipObj)
    {
        Grabbable b = NearestObjUtils.GetNearestGameObject(
            fingerTipObj,
            InteractionWorld.Instance.Grabbables);


        // TODO add limit

        currentlyGrabbing = b;
        b.GrabbingStart(fingerTipObj);
        Debug.Log("Nearest grabbable is at position : " + b.GrabbableObject.transform.parent.gameObject.name);
    }


    public void Update()
    {
        Grabbable b = NearestObjUtils.GetNearestGameObject(
            p.RightHandPinchDetector.FingerTipObj,
            InteractionWorld.Instance.Grabbables);

        if (currentlyHovering != b)
        {
            if (currentlyHovering == null) currentlyHovering = b;

            currentlyHovering.SetIsHovering(false);
            b.SetIsHovering(true);

            currentlyHovering = b;
        }
    }
}