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
        avatar.stopMoving();
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

    // Update is called once per frame
    void Update()
    {
        updateMovement();
        updateJump();
    }
}
