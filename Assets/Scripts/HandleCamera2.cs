using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCamera2 : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera mainCamera;

    private float height;
    private float width;

    void Start()
    {
        height = mainCamera.pixelHeight;
        width = mainCamera.pixelWidth;

    }

    void Update(){
    }

    void OnBecameInvisible(){

        Debug.Log(Mathf.Abs(transform.position.x-mainCamera.transform.position.x));

        if(Mathf.Abs(transform.position.x-mainCamera.transform.position.x)>width/2){
            Vector3 deplacement = transform.position - mainCamera.transform.position;
            mainCamera.transform.position += new Vector3(deplacement.x*2,deplacement.y,0);   
        }else{
            Vector3 deplacement = transform.position - mainCamera.transform.position;
            mainCamera.transform.position += new Vector3(deplacement.x,deplacement.y*2,0);   
        }

    }
}
