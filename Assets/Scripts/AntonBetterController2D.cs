using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonBetterController2D : MonoBehaviour
{
    CharacterMotor motor;
    public float speed = 400f;
    public float jumpForce = 30f;

    float InputX = 0f;
    float stickTimer = 0f;
    float exitTimer = 0f;
    float skinWidth = 0.03f;
    float wallDir = 0f;

    bool huggingWall = false;
    bool isGrounded = true;
    bool doubleJumped = false;
    bool canWallStick = true;

    Vector3 movement;

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    void Update()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        isGrounded = motor.Grounded();
        movement = new Vector2(InputX, 0);

        // Wall detection
        if (isGrounded) canWallStick = true; // when grounded wallstick resets

        if (canWallStick && !isGrounded)
        {  // we cannot stick to the wall if we're grounded
            bool hitWall = motor.CheckObstacle(movement, skinWidth);

            if (hitWall)
            {
                wallDir = Mathf.Sign(movement.x);
                huggingWall = true;
                exitTimer = 0f;
            }
            else if (wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign(movement.x)))
            {
                exitTimer += Time.deltaTime;
                if (exitTimer > 0.6f)
                {
                    huggingWall = false;
                    exitTimer = 0f;
                }
            }
            else
            {
                exitTimer = 0f;
            }

            if (huggingWall)
            {
                stickTimer += Time.deltaTime;
                if (stickTimer > 1f)
                {
                    huggingWall = false;
                }
            }
        }

        motor.FreezeXAxis(huggingWall);
        motor.FreezeYAxis(huggingWall);

        // Movement
        motor.SetVelocity(movement * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            canWallStick = true;
            stickTimer = 0f;
            exitTimer = 0f;
            Jumpy();
        }

    }

    void Jumpy()
    {
        if (isGrounded || !doubleJumped)
        {
            motor.ResetPhysics();
            motor.ApplyForce(new Vector2(0, jumpForce));
            doubleJumped = !isGrounded;
        }
        else if (huggingWall)
        {
            motor.ResetPhysics();
            motor.ApplyForce(new Vector2(10, jumpForce));
            huggingWall = false;
            canWallStick = false;
            StartCoroutine(ResetWallStick());
        }
    }

    IEnumerator ResetWallStick()
    {
        yield return new WaitForSeconds(0.075f);
        canWallStick = true;
    }
}