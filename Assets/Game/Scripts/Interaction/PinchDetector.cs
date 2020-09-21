using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinchDetector : MonoBehaviour
{
    public OVRHand OVRHand;
    public OVRSkeleton OVRSkeleton;
    public GameObject FingerTipObj;

    public bool IsCurrentlyPinching;

    public UnityEvent OnPinchStart = new UnityEvent();
    public UnityEvent OnPinchEnd = new UnityEvent();

    void Awake()
    {
        CreateFingerTipObj();
    }
    
    private void CreateFingerTipObj()
    {
        FingerTipObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        FingerTipObj.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);

        SphereCollider s = FingerTipObj.GetComponent<SphereCollider>();
        s.isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateFingerTipPos();
        DetectPinch();
    }

    private void UpdateFingerTipPos()
    {
        if (OVRSkeleton.Bones.Count > 0)
        {
            var tipPos = OVRSkeleton.Bones[(int) OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
            FingerTipObj.transform.position = tipPos;
        }
    }

    private void DetectPinch()
    {
        bool isPinching = OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (isPinching && !IsCurrentlyPinching)
        {
            IsCurrentlyPinching = true;
            OnPinchStart.Invoke();
        }
        else if (!isPinching && IsCurrentlyPinching)
        {
            IsCurrentlyPinching = false;
            OnPinchEnd.Invoke();
        }
    }
}