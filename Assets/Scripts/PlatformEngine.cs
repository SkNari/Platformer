using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEngine : MonoBehaviour
{   
    private Vector3 startingPos;

    public Vector3 speed;

    public float timeBeforeReverse; // in seconds

    private float timeSinceLastReverse;// in seconds    

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        timeSinceLastReverse = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   
        handleReverse();
        updatePosition();
    }

    void updatePosition(){
        transform.position += speed * Time.deltaTime;
    }

    void handleReverse(){

        if(Time.time - timeSinceLastReverse > timeBeforeReverse){
            speed*=-1;
            timeSinceLastReverse = Time.time;
        }
    }
}
