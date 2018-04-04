using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour {
    CharacterMotor motor;
    public float speed = 400f;
    public float jumpForce = 30f;
    public float wallJumpForce = 5f;

    float InputX = 0f;
    float stickTimer = 0f;
    float exitTimer = 0f;
    float skinWidth = 0.03f;
    float wallDir = 0f;

    bool canAttack = true;
    bool huggingWall = false;
    bool isGrounded = true;
    bool doubleJumped = false;

    Vector3 movement;

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    void Update()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        motor.SetVelocity(movement * speed * Time.deltaTime);
        isGrounded = motor.Grounded();
        movement = new Vector2(InputX, 0);

        if (Input.GetButtonDown("Jump"))
        {
            Jumpy();
            stickTimer = 0f;
            exitTimer = 0f;
            huggingWall = false;
        }

        if (Input.GetButtonDown("Attack"))
        {
            Attack();
            canAttack = false;
        }

        motor.FreezeXAxis(huggingWall);
        motor.FreezeYAxis(huggingWall);
        

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
            if (exitTimer > 0.15f)
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
    void Jumpy()
    {
        if (isGrounded || !doubleJumped)
        {
            motor.ResetPhysics();
            motor.ApplyForce(new Vector2(0, jumpForce));
            doubleJumped = !isGrounded;
        }
        else if (huggingWall && wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign(movement.x)))
        {
            motor.ResetPhysics();
            motor.ApplyForce(new Vector2(-wallDir * wallJumpForce, jumpForce));
            huggingWall = false;
        }
    }

    void Attack()
    {
        if (canAttack)
        {
            Debug.Log("Has attacked");
        }
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(.4f);
        canAttack = true;
    }
}