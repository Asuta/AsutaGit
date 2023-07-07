using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogQuantum : MonoBehaviour
{
    private Transform thisT;
    // Start is called before the first frame update
    void Start()
    {
        thisT = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // log the rotation of thisT as quaternion
        //Debug.Log(thisT.rotation.ToFPQuaternion());
    }
}
