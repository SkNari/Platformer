using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Avatar avatar;

    void move()
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

    void jump()
    {
        if (Input.GetAxis("Jump") == 1f)
        {
            avatar.jump();
        }
    }

    // Update is called once per frame
    void Update()
    {
        move();
        jump();
    }
}
