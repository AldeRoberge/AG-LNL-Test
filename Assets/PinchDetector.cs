using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinchDetector : MonoBehaviour
{
    public OVRHand OVRHand;
    public OVRSkeleton OVRSkeleton;
    protected GameObject FingerTipObj;

    public bool IsCurrentlyPinching;

    public UnityEvent OnPinchStart = new UnityEvent();
    public UnityEvent OnPinchEnd = new UnityEvent();

    void Awake()
    {
        CreateFingerTipObj();
        AddListeners();
    }

    private void AddListeners()
    {
        OnPinchStart.AddListener(() => { Debug.Log("Pinch left start."); });
        OnPinchEnd.AddListener(() => { Debug.Log("Pinch left stop."); });
    }

    private void CreateFingerTipObj()
    {
        FingerTipObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        FingerTipObj.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
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
            var tipPos = OVRSkeleton.Bones[(int) OVRSkeleton.BoneId.Hand_Index3].Transform.position;
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