using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{   

    [SerializeField]
    private bool doubleJump;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private Vector2 speed;

    public Vector2 Speed{
        get{
            return speed;
        }
        set{
            speed = value;
        }
    }

    [SerializeField]
    private float floatHeight;

    public float FloatHeight{
        get{
            return floatHeight;
        }
        set{
            floatHeight= value;
        }
    }

    [SerializeField]
    private float liftForce;

    public float LiftForce{
        get{
            return liftForce;
        }
        set{
            liftForce = value;
        }
    }

    public BaseAvatar avatar;

    public Vector2 Position {
        get{
            return (Vector2)this.transform.position;
        }
        private set{
            this.transform.position = (Vector3)value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        handleGravity();
        handleCollisions();
        move();
    }

    
    
    public void move(){

        Position += speed * avatar.Speed * Time.deltaTime; 

    }

    public void handleGravity(){

        if(speed.y>-2&&!isGrounded){
            speed += Gravity.Reference.Acceleration * Time.deltaTime;
        }

        if(isGrounded){
            speed.y = 0.0f;
        }
    }

    public void handleCollisions(){

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position,-Vector2.up,speed.y*Time.deltaTime+floatHeight);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position,Vector2.up,speed.y*Time.deltaTime+floatHeight);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position,Vector2.left,speed.x*Time.deltaTime+floatHeight);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position,Vector2.right,speed.x*Time.deltaTime+floatHeight);



        if(hitDown.collider != null){
            float distance = Mathf.Abs(hitDown.point.y - transform.position.y);
            float heightError = floatHeight - distance;
            Position += new Vector2(0.0f,heightError);
            isGrounded = true;
            doubleJump = true;
        }else{
            isGrounded = false;
        }
        if(hitUp.collider != null){
            float distance = Mathf.Abs(hitUp.point.y - transform.position.y);
            float heightError = floatHeight - distance;
            Position+= new Vector2(0.0f,-heightError);
            
        }

        if(hitLeft.collider != null){
            float distance = Mathf.Abs(hitLeft.point.x - transform.position.x);
            float heightError = floatHeight - distance;
            Position+= new Vector2(heightError,0.0f);

            
        }

        if(hitRight.collider != null){
            float distance = Mathf.Abs(hitRight.point.x - transform.position.x);
            float heightError = floatHeight - distance;
            Position+= new Vector2(-heightError,0.0f);
        }

    }

    public void Jump(){

        if(doubleJump   &&!isGrounded){
            speed.y = jumpHeight;
            doubleJump = false;
            Position+= new Vector2(0.0f,speed.y)*Time.deltaTime;
        }else if(isGrounded){
            speed.y = jumpHeight;
            isGrounded = false;
            Position+= new Vector2(0.0f,speed.y)*Time.deltaTime;
        }
    }

    public void SetHorizontalSpeed(float x){
        speed.x = x;
    }
}
