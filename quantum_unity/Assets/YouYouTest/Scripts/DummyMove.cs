using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMove : MonoBehaviour
{
    public float highNumber;
    public Transform targetHead;
    private Transform thisT;
    // Start is called before the first frame update
    void Start()
    {
        thisT = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //make thisT is Y position is  targetHead - highNumber
        thisT.position = new Vector3(thisT.position.x, targetHead.position.y - highNumber, thisT.position.z);

    }
}
