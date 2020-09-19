using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaceOnTopOfParent : MonoBehaviour
{
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello! I'm a sphere. My position is " + transform.position);
        parent = this.gameObject.transform.parent.gameObject;
    }

    public void Update()
    {

    }
}