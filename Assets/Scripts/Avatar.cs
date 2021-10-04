using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public float gravity = 0f;
    public float jumpSpeed = 0f;
    public float moveSpeed = 0f;
    public bool onGround = false;
    public bool onRightWall;
    bool onLeftWall;
    bool onCeiling;
    public float wallFallingSpeed;
    public float detectionOffset = 0.01f;
    public float verticalSpeed = 0f;
    public float horizontalSpeed = 0f;
    float playerXSize;
    float playerYSize;

    void detectGround()
    {
        Vector2 rightOffset = new Vector2((playerXSize / 2) - (playerXSize/100), -playerYSize / 2);
        RaycastHit2D hitsRight = Physics2D.Raycast((Vector2)transform.position + rightOffset, Vector2.down, -verticalSpeed * Time.deltaTime + detectionOffset);
        if (hitsRight.collider != null)
        {
            Vector3 vertical = new Vector3(0f, -hitsRight.distance, 0f);
            transform.position += vertical;
            onGround = true;
            return;
        }
        else
        {
            Vector2 leftOffset = new Vector2((-playerXSize / 2) + (playerXSize/100), -playerYSize / 2);
            RaycastHit2D hitsLeft = Physics2D.Raycast((Vector2)transform.position + leftOffset, Vector2.down, -verticalSpeed * Time.deltaTime + detectionOffset);
            if (hitsLeft.collider != null)
            {
                Vector3 vertical = new Vector3(0f, -hitsLeft.distance, 0f);
                transform.position += vertical;
                onGround = true;
                return;
            }
        }
        onGround = false;
    }

    void detectCeiling()
    {
        Vector2 rightOffset = new Vector2((playerXSize / 2) - (playerXSize / 100), playerYSize / 2);
        RaycastHit2D hitsRight = Physics2D.Raycast((Vector2)transform.position + rightOffset, Vector2.up, verticalSpeed * Time.deltaTime + detectionOffset);
        if (hitsRight.collider != null)
        {
            Vector3 vertical = new Vector3(0f, hitsRight.distance, 0f);
            transform.position += vertical;
            onCeiling = true;
            return;
        }
        else
        {
            Vector2 leftOffset = new Vector2((-playerXSize / 2) + (playerXSize / 100), playerYSize / 2);
            RaycastHit2D hitsLeft = Physics2D.Raycast((Vector2)transform.position + leftOffset, Vector2.up, verticalSpeed * Time.deltaTime + detectionOffset);
            if (hitsLeft.collider != null)
            {
                Vector3 vertical = new Vector3(0f, hitsLeft.distance, 0f);
                transform.position += vertical;
                onCeiling = true;
                return;
            }
        }
        onCeiling = false;
    }

    void detectRightWall()
    {
        Vector2 upOffset = new Vector2(playerXSize / 2, (playerYSize / 2) - (playerYSize / 100));
        RaycastHit2D hitsUp = Physics2D.Raycast((Vector2)transform.position + upOffset, Vector2.right, horizontalSpeed * Time.deltaTime + detectionOffset);
        if (hitsUp.collider != null)
        {
            Vector3 horizontal = new Vector3(hitsUp.distance, 0f, 0f);
            transform.position += horizontal;
            onRightWall = true;
            return;
        }
        else
        {
            Vector2 downOffset = new Vector2(playerXSize / 2, -((playerYSize / 2) - (playerYSize / 100)));
            RaycastHit2D hitsDown = Physics2D.Raycast((Vector2)transform.position + downOffset, Vector2.right, horizontalSpeed * Time.deltaTime + detectionOffset);
            if (hitsDown.collider != null)
            {
                Vector3 horizontal = new Vector3(hitsDown.distance, 0f, 0f);
                transform.position += horizontal;
                onRightWall = true;
                return;
            }
        }
        onRightWall = false;
    }

    void detectLeftWall()
    {
        Vector2 upOffset = new Vector2(-playerXSize / 2, (playerYSize / 2) - (playerYSize / 100));
        RaycastHit2D hitsUp = Physics2D.Raycast((Vector2)transform.position + upOffset, Vector2.left, -horizontalSpeed * Time.deltaTime + detectionOffset);
        if (hitsUp.collider != null)
        {
            Vector3 horizontal = new Vector3(-hitsUp.distance, 0f, 0f);
            transform.position += horizontal;
            onLeftWall = true;
            return;
        }
        else
        {
            Vector2 downOffset = new Vector2(-playerXSize / 2, -((playerYSize / 2) - (playerYSize / 100)));
            RaycastHit2D hitsDown = Physics2D.Raycast((Vector2)transform.position + downOffset, Vector2.left, -horizontalSpeed * Time.deltaTime + detectionOffset);
            if (hitsDown.collider != null)
            {
                Vector3 horizontal = new Vector3(-hitsDown.distance, 0f, 0f);
                transform.position += horizontal;
                onLeftWall = true;
                return;
            }
        }
        onLeftWall = false;
    }

    void fall()
    {
        verticalSpeed -= gravity * Time.deltaTime;
        detectGround();
        if (verticalSpeed <= 0)
        {
            if (onGround)
            {
                verticalSpeed = 0f;
            }
            else
            {
                if (onRightWall || onLeftWall)
                {
                    if (verticalSpeed < -wallFallingSpeed)
                    {
                        verticalSpeed = -wallFallingSpeed;
                    }
                }
                Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                transform.position += vertical * Time.deltaTime;
            }
        }
        else
        {
            detectCeiling();
            if(onCeiling)
            {
                verticalSpeed = 0f;
            }
            else
            {
                Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                transform.position += vertical * Time.deltaTime;
            }
        }
    }

    public void jump()
    {
        if (onGround)
        {
            verticalSpeed = jumpSpeed;
            Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
            transform.position += vertical *Time.deltaTime;
        }
    }

    void updateHorizontal()
    {
        Vector3 horizontal = new Vector3(horizontalSpeed, 0f, 0f);
        transform.position += horizontal * Time.deltaTime;
    }

    public void goRight(float input)
    {
        horizontalSpeed = moveSpeed * input;
        detectRightWall();
        if (onRightWall)
        {
            stopMoving();
        }
    }

    public void goLeft(float input)
    {
        horizontalSpeed = moveSpeed * input;
        detectLeftWall();
        if (onLeftWall)
        {
            stopMoving();
        }
    }

    public void stopMoving()
    {
        horizontalSpeed = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerXSize = transform.localScale.x;
        playerYSize = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        fall();
        updateHorizontal();
    }
}
