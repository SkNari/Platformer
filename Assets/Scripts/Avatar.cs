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
    bool isDashing = false;
    public GameObject particlesLot;
    public GameObject particlesFew;
    public Vector3 particlesLandOffset;
    side dashDirection;
    public float dashDuration;
    public float dashSpeed;
    public float dashCooldown;
    float dashesLeft;
    public float maxDashes;
    float timeOfLastDash = -100f;
    float timeSinceBeginningOfDash;
    public int maxJumps;
    int jumpsLeft;
    bool onGround = false;
    bool onRightWall;
    bool onLeftWall;
    bool onRightWallLastFrame;
    bool onLeftWallLastFrame;
    side lastWallJump = side.NONE;
    bool onCeiling;
    public float wallFallingSpeed;
    public float detectionOffset = 0.01f;
    float verticalSpeed = 0f;
    float horizontalSpeed = 0f;
    float playerXSize;
    float playerYSize;
    bool isForcingFall = false;

    //jumpVariables
    bool isJumping = false;
    public float jumpDuration;
    public float jumpMaxspeed;
//    float timeOfJump = 0.0f;

    //stickyPlatforms handle variables

    float stickyHorizontalMultiplier;
    float stickyJumpMultiplier;

    Vector3 platformSpeed = new Vector3(0,0,0);

    GameObject wallTouched;

    public bool IsForcingFall {
        set{
            isForcingFall = value;
        }
        get{
            return isForcingFall;
        }
    }
    public float forcingFallSpeed;

    public GameObject spawnPoint;
    public CameraShake cameraShake;

    //camerashake

    public CameraShake shaker;

    void detectGround()
    {
        Vector2 rightOffset = new Vector2((playerXSize / 2) - (playerXSize/100), -playerYSize / 2);
        RaycastHit2D hitsRight = Physics2D.Raycast((Vector2)transform.position + rightOffset, Vector2.down, -verticalSpeed * Time.deltaTime + detectionOffset);
        if (hitsRight.collider != null)
        {   
            wallTouched = hitsRight.collider.gameObject;
            Vector3 vertical = new Vector3(0f, -hitsRight.distance, 0f);
            transform.position += vertical;
            onGround = true;
            jumpsLeft = maxJumps;
            dashesLeft = maxDashes;
            lastWallJump = side.NONE;
            return;
        }
        else
        {
            Vector2 leftOffset = new Vector2((-playerXSize / 2) + (playerXSize/100), -playerYSize / 2);
            RaycastHit2D hitsLeft = Physics2D.Raycast((Vector2)transform.position + leftOffset, Vector2.down, -verticalSpeed * Time.deltaTime + detectionOffset);
            if (hitsLeft.collider != null)
            {   
                wallTouched = hitsLeft.collider.gameObject;
                Vector3 vertical = new Vector3(0f, -hitsLeft.distance, 0f);
                transform.position += vertical;
                onGround = true;
                jumpsLeft = maxJumps;
                dashesLeft = maxDashes;
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
            wallTouched = hitsRight.collider.gameObject;
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
                wallTouched = hitsLeft.collider.gameObject;
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
            if (wallTouched == null || wallTouched.tag != "KillZone")
            {
                wallTouched = hitsUp.collider.gameObject;
            }
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
                if (wallTouched == null || wallTouched.tag != "KillZone")
                {
                    wallTouched = hitsDown.collider.gameObject;
                }
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
            if (wallTouched == null || wallTouched.tag != "KillZone")
            {
                wallTouched = hitsUp.collider.gameObject;
            }
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
                if (wallTouched == null || wallTouched.tag != "KillZone")
                {
                    wallTouched = hitsDown.collider.gameObject;
                }
                Vector3 horizontal = new Vector3(-hitsDown.distance, 0f, 0f);
                transform.position += horizontal;
                onLeftWall = true;
                return;
            }
        }
        onLeftWall = false;
    }

    void detectWalls(){
        detectGround();
        detectCeiling();
        detectLeftWall();
        detectRightWall();
    }
    
    void land()
    {
        if (verticalSpeed < -40f)
        {
            particlesLot.transform.position = transform.position + particlesLandOffset;
            particlesLot.GetComponent<ParticleSystem>().Play();
        }
        if (verticalSpeed < -10f)
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
            if(!isJumping){
                verticalSpeed -= isForcingFall ? gravity * forcingFallSpeed * Time.deltaTime : gravity * Time.deltaTime;
            }
            if (verticalSpeed <= 0)
            {
                if (onGround)
                {
                    if (verticalSpeed <= 0f)
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
                verticalSpeed = jumpSpeed * stickyJumpMultiplier;
                Vector3 vertical = new Vector3(0f, verticalSpeed, 0f);
                transform.position += vertical * Time.deltaTime;
            }
            else
            {
                if (onRightWall && lastWallJump != side.RIGHT)//walljump on right wall
                {
                    verticalSpeed = jumpSpeed * stickyJumpMultiplier;
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
                        verticalSpeed = jumpSpeed * stickyJumpMultiplier;
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
                            verticalSpeed = jumpSpeed * stickyJumpMultiplier;
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
        if (!isDashing && timeOfLastDash + dashCooldown < Time.time && dashesLeft > 0)
        {
            dashesLeft--;
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
        transform.position += (horizontal+platformSpeed) * Time.deltaTime * stickyHorizontalMultiplier;
    }

    public void goRight(float input)
    {
        if (!isDashing)
        {
            if (onGround)
            {
                horizontalSpeed = maxHorizontalSpeed*input;
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
                horizontalSpeed = maxHorizontalSpeed*input;
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

    void updateWallTouched(){

        if(!onCeiling&&!onGround&&!onLeftWall&&!onRightWall){
            wallTouched = null;
        }

    }

    // update : input -> bool
    // fixedeupdate -> bool,horizontal axis -> ajout de vélocité(vector2) si on dépasse pas maxspeed
    // jump : velocité.y += jump
    //             -> checkcollisions -> si dans un truc, on sort
    //                                 -> si touche un mur, vélocité correspondante à 0
    //                 move -> vector2 vélocité -> en x, velocité.x * time.fixedDeltaTime
    //                                         -> en y, velocité.y * time.fixeddeltatime + -1/2 g * temps²
                                                





    void handleMovingPlatform(){

        if((onLeftWall||onRightWall||onGround)&&wallTouched.tag=="MovingPlatform"){
            PlatformEngine platform = wallTouched.GetComponent<PlatformEngine>();
            if(!(onRightWall&&platform.speed.x>0)&&!(onLeftWall&&platform.speed.x<0)){
                platformSpeed = platform.speed;
            }
        }else{
            platformSpeed = new Vector3(0,0,0);
        }

    }

    void  handleKillZone(){
        if((onLeftWall||onRightWall||onGround||onCeiling)&&wallTouched.tag=="KillZone"){
            kill();
        }
    }

    void handleStickyPlatform(){
        if(onGround&&wallTouched.tag=="StickyPlatform"){
            StickyPlatform platform = wallTouched.GetComponent<StickyPlatform>();
            stickyHorizontalMultiplier = platform.horizontalSpeedSlowMultiplier;
            stickyJumpMultiplier = platform.jumpSlowMultiplier;
        }else{
            stickyHorizontalMultiplier = 1.0f;
            stickyJumpMultiplier = 1.0f;
        }
    }

    void handleExitDoor(){

        if((onLeftWall||onRightWall||onGround||onCeiling)&&wallTouched.tag=="ExitDoor"){
            ExitDoor platform = wallTouched.GetComponent<ExitDoor>();
            platform.exit();
        }

    }

    // void handleJump(){
    //     Vector3 jumpVector = new Vector3(0,0,0);
    //     if(isJumping&&timeOfJump<jumpDuration/2){

    //         verticalSpeed = jumpMaxspeed - jumpMaxspeed*(timeOfJump/jumpDuration);

    //         timeOfJump+=Time.deltaTime;

    //         Debug.Log("je monte");

    //     }else if(isJumping&&jumpDuration/2<timeOfJump&&timeOfJump<jumpDuration){
    //         verticalSpeed = -jumpMaxspeed+jumpMaxspeed*((timeOfJump+jumpDuration/2)/jumpDuration);
    //         timeOfJump+=Time.deltaTime;

    //         Debug.Log("Je descend");
    //     }
    //     else{
    //         isJumping=false;
    //     }
    // }

    void kill(){
        transform.position = spawnPoint.transform.position;
        cameraShake.TriggerShake();
    }

    // Start is called before the first frame update
    void Start()
    {   
        playerXSize = transform.localScale.x;
        playerYSize = transform.localScale.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        detectWalls();
        updateWallTouched();
        handleMovingPlatform();
        handleKillZone();
        handleStickyPlatform();
        handleExitDoor();
        fall();
        updateHorizontal();
        updateDash();

    }
}
