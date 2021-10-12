using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Avatar avatar;
    bool jumpPressedLastFrame;
    void updateMovement()
    {
        float input = Input.GetAxis("Horizontal");
        if (input > 0)
        {
            avatar.goRight(input);
            return;
        }
        if (input < 0)
        {
            avatar.goLeft(input);
            return;
        }
        avatar.slowDown();
    }

    void updateJump()
    {
        float input = Input.GetAxis("Jump");
        if (input == 1f && !jumpPressedLastFrame)
        {
            avatar.jump();
        }
        jumpPressedLastFrame = (input == 1f);
    }

    void updateInterruptJump()
    {
        if (Input.GetAxis("Jump") == 0f && jumpPressedLastFrame)
        {
            avatar.interruptJump();
        }

    }

    void updateDash()
    {
        if(Input.GetAxis("Fire1") == 1f)
        {
            if (Input.GetAxis("Horizontal") > 0f)
            {
                avatar.dash(side.RIGHT);
            }
            if (Input.GetAxis("Horizontal") < 0f)
            {
                avatar.dash(side.LEFT);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();
        updateInterruptJump();
        updateJump();
        updateDash();
    }
}
