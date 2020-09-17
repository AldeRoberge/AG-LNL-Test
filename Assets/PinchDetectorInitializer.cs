using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Inits and links PinchDetectors to their given hands.
/// </summary>
public class PinchDetectorInitializer : MonoBehaviour
{
    [HideInInspector] public PinchDetector LeftHandPinchDetector;
    [HideInInspector] public PinchDetector RightHandPinchDetector;

    void Awake()
    {
        RegisterLeftHand();
        RegisterRightHand();
    }

    private void RegisterLeftHand()
    {
        LeftHandPinchDetector = gameObject.AddComponent<PinchDetector>();
        LeftHandPinchDetector.OVRHand = GetOVRHand(OVRHand.Hand.HandLeft);
        LeftHandPinchDetector.OVRSkeleton = GetOVRSkeleton(OVRSkeleton.SkeletonType.HandLeft);
    }

    private void RegisterRightHand()
    {
        RightHandPinchDetector = gameObject.AddComponent<PinchDetector>();
        RightHandPinchDetector.OVRHand = GetOVRHand(OVRHand.Hand.HandRight);
        RightHandPinchDetector.OVRSkeleton = GetOVRSkeleton(OVRSkeleton.SkeletonType.HandRight);
    }

    /// <summary>
    /// Utility to get the OVRHand for a given hand (left or right)
    /// </summary>
    private OVRHand GetOVRHand(OVRHand.Hand handType)
    {
        foreach (var hand in GetComponentsInChildren<OVRHand>())
            if (hand.HandType == handType)
                return hand;

        Debug.LogError("[PinchManager] Could not get OVRHand with hand type '" + handType + "'.");
        return null;
    }

    /// <summary>
    /// Utility to get the OVRSkeleton for a given hand (left or right)
    /// </summary>
    private OVRSkeleton GetOVRSkeleton(OVRSkeleton.SkeletonType skeletonType)
    {
        foreach (var skeleton in GetComponentsInChildren<OVRSkeleton>())
            if (skeleton.GetSkeletonType() == skeletonType)
                return skeleton;

        Debug.LogError("[PinchManager] Could not get OVRSkeleton with skeleton type '" + skeletonType + "'.");
        return null;
    }
}