using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCamera1 : MonoBehaviour
{   
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
       Vector3 desiredPosition = new Vector3(target.position.x,target.position.y,transform.position.z) + offset;
       Vector3 smoothedPosition = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
       transform.position = smoothedPosition;
    }
}
