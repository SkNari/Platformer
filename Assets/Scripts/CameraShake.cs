using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{


    private float shakeDuration = 0f;
    
    public float shakeMagnitude;

    public float dampingSpeed;

    Vector3 initialPosition;

    void OnEnable(){
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeDuration>0)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration-=Time.deltaTime*dampingSpeed;
        }else{
            shakeDuration=0.0f;
            transform.position = initialPosition;
            //Debug.Log(initialPosition.y);
        }
    }

    public void TriggerShake(){
        shakeDuration = 0.5f;
    }
}
