using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControlller : MonoBehaviour
{   

    [SerializeField]
    private Engine engine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");

        engine.SetHorizontalSpeed(xAxis);

        if(Input.GetAxis("Jump")>0.0f){
            engine.Jump();
        }
    }
}
