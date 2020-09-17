using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureDetector : MonoBehaviour
{
    
    public OVRHand OVRHand;
    public OVRSkeleton OVRSkeleton;
    
    public GameObject FingerTipObj;
    
    public bool IsCurrentlyPinching;
    
    public UnityEvent OnPinchStart = new UnityEvent();
    public UnityEvent OnPinchEnd = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        OnPinchStart.AddListener(() => { Debug.Log("Pinching left."); });
        OnPinchEnd.AddListener(() => { Debug.Log("Unpinching left."); });
    }

    // Update is called once per frame
    void Update()
    {

        try
        {
            var tipPos = OVRSkeleton.Bones[(int) OVRSkeleton.BoneId.Hand_Index3].Transform.position;
            FingerTipObj.transform.position = tipPos;

            Debug.Log("Yeah");
        }
        catch (Exception e)
        {
            Debug.Log("Nope");
        }
        
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