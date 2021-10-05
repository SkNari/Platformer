using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{   
    [SerializeField]
    private Vector2 acceleration;

    public Vector2 Acceleration{
        get{
            return acceleration;
        }
        set{
            acceleration = value;
        }
    }

    public static Gravity Reference {get;private set;}

    void Awake()
    {
        if(Reference==null){
            Reference = this;
        }else{
            Destroy(this);
        }
    }

}
