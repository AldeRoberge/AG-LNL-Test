using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PinchDetectorInitializer p = gameObject.AddComponent<PinchDetectorInitializer>();

        p.LeftHandPinchDetector.OnPinchStart.AddListener(() =>
        {
            Debug.Log("Left hand pinched!");
        });
        
        p.RightHandPinchDetector.OnPinchStart.AddListener(() =>
        {
            Debug.Log("Right hand pinched!");
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}