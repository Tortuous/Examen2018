using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController2D : MonoBehaviour
{
    public float InputX;
    public float InputY;
    public float speed = 6f;
    public float jumpForce = 500f;
    public Animator animator;
    public Text score;

    public List<GameObject> nearTarget = new List<GameObject>();

    static public int targetCount = 0;
    bool isGrounded = true;
    bool Attacking = false;
    bool Shooting = false;
    const float locoST = .1f;
    int maxJumps = 2;
    int jumps;
    Vector3 movement;

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");
        animator.SetFloat("InputY", InputY, locoST, Time.deltaTime);
        animator.SetFloat("InputX", InputX, locoST, Time.deltaTime);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("Attacking", Attacking);
        animator.SetBool("Shooting", Shooting);
        Attacking = false;
        Shooting = false;
        transform.Translate(InputX * Time.deltaTime * speed, 0, 0, Space.World);
        movement = new Vector3(InputX, 0, 0);
        transform.rotation = Quaternion.LookRotation(-movement);

        score.text = targetCount.ToString();

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
    // controls edit
    void Attack()
    {
        if (!Attacking && nearTarget.Count != 0)
        {
            Debug.Log("Hit");
            Attacking = true;
            targetCount++;
            for (int i = 0; i > 0; i--)
            {
                nearTarget[i].SetActive(false);
            }
        }
        else if (!Attacking && nearTarget.Count == 0)
        {
            Debug.Log("Attacked , but missed");
            Attacking = true;
        }
        else
        {
            return;
        }

    }

    void Shoot()
    {
        if (!Shooting)
        {
            Shooting = true;
        }
        else
        {
            return;
        }
    }

    void Jump()
    {
        if (jumps > 0)
        {
            Debug.Log("Jumped once");
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            isGrounded = false;
            jumps = jumps - 1;
        }
        else
        {
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "target")
        {
            nearTarget.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "target")
        {
            nearTarget.Remove(col.gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collide)
    {
        if (collide.gameObject.GetComponent<Platform>())
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