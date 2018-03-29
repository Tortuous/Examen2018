using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    public float speed = 8f;
    public float jumpForce;
    public Animator animator;

    bool isGrounded = true;
    bool Attacking = false;
    const float locoST = .1f;
    public float InputX;
    public float InputY;
    int maxJumps = 2;
    int jumps;
    Vector3 movement;

    void Update ()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");
        animator.SetFloat("InputY", InputY, locoST, Time.deltaTime);
        animator.SetFloat("InputX", InputX, locoST, Time.deltaTime);
        animator.SetBool("isGrounded", isGrounded);
        
        transform.Translate(InputX * Time.deltaTime * speed, 0, 0, Space.World);
        movement = new Vector3(InputX, 0, 0);
        transform.rotation = Quaternion.LookRotation(-movement);
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }

        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }

    void Attack()
    {
        if (!Attacking)
        {
            Attacking = true;
            animator.SetBool("Attacking", Attacking);
        }
        else
        {
            return;
        }
    }

    void Shoot()
    {
        if (!Attacking)
        {
            Attacking = true;
            animator.SetBool("Attacking", Attacking);
        }
        else
        {
            return;
        }
    }

    void Jump()
    {
        if(jumps > 0)
        {
            Debug.Log("Jumped once");
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