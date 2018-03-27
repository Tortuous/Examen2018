using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 8f;
    public float jumpForce;

    int maxJumps = 2;
    int jumps;
    static bool isGrounded = true;

    void Update ()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, 0);

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