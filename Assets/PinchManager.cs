using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchManager : MonoBehaviour
{
    private PinchDetector LeftHandPinch;
    private PinchDetector RightHandPinch;

    void Start()
    {
        RegisterLeftHand();
        RegisterRightHand();
    }

    private void RegisterLeftHand()
    {
        LeftHandPinch = gameObject.AddComponent<PinchDetector>();
        LeftHandPinch.OVRHand = GetOVRHand(OVRHand.Hand.HandLeft);
        LeftHandPinch.OVRSkeleton = GetOVRSkeleton(OVRSkeleton.SkeletonType.HandLeft);
    }

    private void RegisterRightHand()
    {
        RightHandPinch = gameObject.AddComponent<PinchDetector>();
        RightHandPinch.OVRHand = GetOVRHand(OVRHand.Hand.HandRight);
        RightHandPinch.OVRSkeleton = GetOVRSkeleton(OVRSkeleton.SkeletonType.HandRight);
    }

    /// <summary>
    /// Utility to get the OVRHand for a given hand (left, right)
    /// </summary>
    private OVRHand GetOVRHand(OVRHand.Hand handType)
    {
        foreach (var hand in GetComponentsInChildren<OVRHand>())
            if (hand.HandType == handType)
                return hand;

        Debug.LogError("[PinchManager] Could not get hand with hand type '" + handType + "'.");
        return null;
    }

    /// <summary>
    /// Utility to get the OVRSkeleton for a given hand (left, right)
    /// </summary>
    private OVRSkeleton GetOVRSkeleton(OVRSkeleton.SkeletonType skeletonType)
    {
        foreach (var skeleton in GetComponentsInChildren<OVRSkeleton>())
            if (skeleton.GetSkeletonType() == skeletonType)
                return skeleton;

        Debug.LogError("[PinchManager] Could not get hand with skeleton type '" + skeletonType + "'.");
        return null;
    }
}