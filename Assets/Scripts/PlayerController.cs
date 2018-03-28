using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    public float speed = 8f;
    public float jumpForce;
    public Animator animator;

    float InputX;
    float InputY;
    const float locoST = .1f;

    int maxJumps = 2;
    int jumps;
    bool isGrounded = true;

    void Update ()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");
        animator.SetFloat("InputY", InputY, locoST, Time.deltaTime);
        animator.SetFloat("InputX", InputX, locoST, Time.deltaTime);
        animator.SetBool("isGrounded", isGrounded);


        transform.Translate(InputX * Time.deltaTime * speed, 0, 0);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        if(jumps > 0)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpForce));
            isGrounded = false;
            jumps = jumps - 1;
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter(Collision collide)
    {
        if(collide.gameObject.GetComponent<Platform>())
        {
            jumps = maxJumps;
            isGrounded = true;
        }

        if (collide.gameObject.GetComponent<Wall>())
        {
            jumps++;
            isGrounded = false;
        }
    }
}