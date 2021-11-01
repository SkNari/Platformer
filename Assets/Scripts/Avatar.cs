using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum side
{
    RIGHT,
    LEFT,
    NONE,
}

public class Avatar : MonoBehaviour
{   
    public float maxHorizontalSpeed = 0f;
    public float horizontalAcceleration = 0.0f;
    public float gravity = 0f;
    public float jumpSpeed = 0f;
    public float wallJumpSpeedMultiplier;
    public bool isDashing = false;
    public GameObject particlesLot;
    public GameObject particlesFew;
    public Vector3 particlesLandOffset;
    side dashDirection;
    public float dashDuration;
    public float dashSpeed;
    public float dashCooldown;
    public float timeOfLastDash = -100f;
    public float timeSinceBeginningOfDash;
    public int maxJumps;
    public int jumpsLeft;
    public bool onGround = false;
    public bool onRightWall;
    public bool onLeftWall;
    bool onRightWallLastFrame;
    bool onLeftWallLastFrame;
    side lastWallJump = side.NONE;
    bool onCeiling;
    public float wallFallingSpeed;
    public float detectionOffset = 0.01f;
    public float verticalSpeed = 0f;
    float horizontalSpeed = 0f;
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
            jumpsLeft = maxJumps;
            lastWallJump = side.NONE;
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
                jumpsLeft = maxJumps;
                lastWallJump = side.NONE;
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

    void land()
    {
        if (verticalSpeed < -5f)
        {
            particlesLot.transform.position = transform.position + particlesLandOffset;
            particlesLot.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            particlesFew.transform.position = transform.position + particlesLandOffset;
            particlesFew.GetComponent<ParticleSystem>().Play();
        }
    }

    void rightWallParticles()
    {
        if (onRightWall && !onRightWallLastFrame)
        {
            Vector3 offset = new Vector3(playerXSize, 0f, 0f);
            particlesFew.transform.position = transform.position + offset;
            particlesFew.GetComponent<ParticleSystem>().Play();
        }
        onRightWallLastFrame = onRightWall;
    }

    void leftWallParticles()
    {
        if (onLeftWall && !onLeftWallLastFrame)
        {
            Vector3 offset = new Vector3(playerXSize, 0f, 0f);
            particlesFew.transform.position = transform.position - offset;
            particlesFew.GetComponent<ParticleSystem>().Play();
        }
        onLeftWallLastFrame = onLeftWall;
    }

    void fall()
    {
        if (!isDashing)
        {
            verticalSpeed -= gravity * Time.deltaTime;
            detectGround();
            if (verticalSpeed <= 0)
            {
                if (onGround)
                {
                    if (verticalSpeed <= -1f)
                    {
                        land();
                    }
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
                if (onCeiling)
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
    }

    public void jump()
    {
        if (!isDashing)
        {
            if (onGround)
            {
                verticalSpeed = jumpSpeed;
                Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                transform.position += vertical * Time.deltaTime;
            }
            else
            {
                if (onRightWall && lastWallJump != side.RIGHT)//walljump on right wall
                {
                    verticalSpeed = jumpSpeed;
                    horizontalSpeed = -maxHorizontalSpeed * wallJumpSpeedMultiplier;
                    Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                    transform.position += vertical * Time.deltaTime;
                    lastWallJump = side.RIGHT;
                    jumpsLeft--;
                }
                else
                {
                    if (onLeftWall && lastWallJump != side.LEFT)//walljump on left wall
                    {
                        verticalSpeed = jumpSpeed;
                        horizontalSpeed = maxHorizontalSpeed * wallJumpSpeedMultiplier;
                        Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                        transform.position += vertical * Time.deltaTime;
                        lastWallJump = side.LEFT;
                        jumpsLeft--;
                    }
                    else
                    {
                        if (jumpsLeft > 0)
                        {
                            verticalSpeed = jumpSpeed;
                            Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                            transform.position += vertical * Time.deltaTime;
                            jumpsLeft--;
                        }
                    }
                }
            }
        }
    }

    public void interruptJump()
    {
        if(verticalSpeed > 0f && jumpsLeft == maxJumps)
        {
            verticalSpeed = 0f;
        }
    }

    public void dash(side direction)
    {
        if (!isDashing && timeOfLastDash + dashCooldown < Time.time && jumpsLeft > 0)
        {
            jumpsLeft--;
            timeSinceBeginningOfDash = 0f;
            isDashing = true;
            dashDirection = direction;
            verticalSpeed = 0f;
        }
    }

    void updateDash()
    {
        if(isDashing)
        {
            if (dashDirection == side.LEFT)
            {
                horizontalSpeed = -dashSpeed * (1 - 0.5f * timeSinceBeginningOfDash / dashDuration);
            }
            if (dashDirection == side.RIGHT)
            {
                horizontalSpeed = dashSpeed * (1 - 0.5f * timeSinceBeginningOfDash / dashDuration);
            }
            if (timeSinceBeginningOfDash > dashDuration || onRightWall || onLeftWall)
            {
                isDashing = false;
                timeOfLastDash = Time.time;
            }
            timeSinceBeginningOfDash += Time.deltaTime;
        }
    }

    void updateHorizontal()
    {
        detectLeftWall();
        detectRightWall();
        rightWallParticles();
        leftWallParticles();
        if (onRightWall && horizontalSpeed > 0f)
        {
            stopMoving();
        }
        if (onLeftWall && horizontalSpeed < 0f)
        {
            stopMoving();
        }
        Vector3 horizontal = new Vector3(horizontalSpeed, 0f, 0f);
        transform.position += horizontal * Time.deltaTime;
    }

    public void goRight(float input)
    {
        if (!isDashing)
        {
            if (onGround)
            {
                horizontalSpeed = maxHorizontalSpeed;
            }
            else
            {
                if (horizontalSpeed > maxHorizontalSpeed)
                {
                    slowDown();
                }
                else
                {
                    horizontalSpeed += horizontalAcceleration * input * Time.deltaTime;
                }
            }
            detectRightWall();
            if (onRightWall)
            {
                stopMoving();
            }
        }
    }

    public void goLeft(float input)
    {
        if (!isDashing)
        {
            if (onGround)
            {
                horizontalSpeed = -maxHorizontalSpeed;
            }
            else
            {
                if (horizontalSpeed < -maxHorizontalSpeed)
                {
                    slowDown();
                }
                horizontalSpeed += horizontalAcceleration * input * Time.deltaTime;
            }
            detectLeftWall();
            if (onLeftWall)
            {
                stopMoving();
            }
            else
            {
                detectRightWall();
            }
        }
    }

    void stopMoving()
    {
        horizontalSpeed = 0f;
    }

    public void slowDown()
    {
        if (!isDashing)
        {
            if (horizontalSpeed > 0f)
            {
                horizontalSpeed -= horizontalAcceleration * Time.deltaTime;
                if (horizontalSpeed < 0f)
                {
                    horizontalSpeed = 0f;
                }
            }
            if (horizontalSpeed < 0f)
            {
                horizontalSpeed += horizontalAcceleration * Time.deltaTime;
                if (horizontalSpeed > 0f)
                {
                    horizontalSpeed = 0f;
                }
            }
        }
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
        updateDash();
    }
}
