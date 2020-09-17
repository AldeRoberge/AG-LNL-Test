using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureDetector : MonoBehaviour
{
    
    public OVRSkeleton RightHandSkeleton;
    public OVRSkeleton LeftHandSkeleton;

    public GameObject ToMove;
    public GameObject ToMove2;
    

    private OVRHand _ovrLeftHand;
    public bool IsCurrentlyPinchingLeft;

    public UnityEvent OnPinchLeft = new UnityEvent();
    public UnityEvent OnUnpinchLeft = new UnityEvent();


    private OVRHand _ovrRightHand;
    public bool IsCurrentlyPinchingRight;

    public UnityEvent OnPinchRight = new UnityEvent();
    public UnityEvent OnUnpinchRight = new UnityEvent();


    public Vector3 RightPointPos;

    public Vector3 LeftPointPos;


    // Start is called before the first frame update
    void Start()
    {
        OnPinchLeft.AddListener(() => { Debug.Log("Pinching left."); });

        OnPinchRight.AddListener(() => { Debug.Log("Pinching right."); });

        OnUnpinchLeft.AddListener(() => { Debug.Log("Unpinching left."); });

        OnUnpinchRight.AddListener(() => { Debug.Log("Unpinching right."); });


        foreach (OVRHand hand in GetComponentsInChildren<OVRHand>())
        {
            switch (hand.HandType)
            {
                case OVRHand.Hand.HandLeft:
                    _ovrLeftHand = hand;
                    break;
                case OVRHand.Hand.HandRight:
                    _ovrRightHand = hand;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        try
        {
            RightPointPos = RightHandSkeleton.Bones[(int) OVRSkeleton.BoneId.Hand_Index3].Transform.position;
            LeftPointPos = LeftHandSkeleton.Bones[(int) OVRSkeleton.BoneId.Hand_Index3].Transform.position;

            ToMove.transform.position = RightPointPos;
            ToMove2.transform.position = LeftPointPos;

            Debug.Log("Yeah");
        }
        catch (Exception e)
        {
            Debug.Log("Nope");
        }
        
        bool isLeftHandPinching = _ovrLeftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (isLeftHandPinching && !IsCurrentlyPinchingLeft)
        {
            IsCurrentlyPinchingLeft = true;
            OnPinchLeft.Invoke();
        }
        else if (!isLeftHandPinching && IsCurrentlyPinchingLeft)
        {
            IsCurrentlyPinchingLeft = false;
            OnUnpinchLeft.Invoke();
        }

        bool isRightHandPinching = _ovrRightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (isRightHandPinching && !IsCurrentlyPinchingRight)
        {
            IsCurrentlyPinchingRight = true;
            OnPinchRight.Invoke();
        }
        else if (!isRightHandPinching && IsCurrentlyPinchingRight)
        {
            IsCurrentlyPinchingRight = false;
            OnUnpinchRight.Invoke();
        }
    }
}