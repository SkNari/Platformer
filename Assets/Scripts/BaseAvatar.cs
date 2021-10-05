using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAvatar : MonoBehaviour
{   

    [SerializeField]
    private float speed;

    public float Speed {
        get{
            return this.speed;
        }
        private set{
            this.speed =value;
        }
    }

    public virtual void kill(){
        Destroy(this.gameObject);
    }

}
