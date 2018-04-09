using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller2D : MonoBehaviour {
    // public vars
    public float speed = 400f;
    public float jumpForce = 30f;
    public float wallJumpForce = 5f;
    public Animator animator;
    public Text score;
    
    static public int targetCount = 0;
    public int targetCount_;

    LayerMask EnemyLayer;
    [SerializeField]
    string jumpAxis;
    [SerializeField]
    string horizontalAxis;
    //[SerializeField]
    //string verticalAxis;
    [SerializeField]
    string attackAxis;
    [SerializeField]
    string shootAxis;

    // private vars
    const float locoST = .1f;
    float InputX = 0f;
    float InputY = 0f;
    float stickTimer = 0f;
    float exitWallTimer = 0f;
    float skinWidth = 0.03f;
    float attackwidth = 1.5f;
    float shootWidth = 20f;
    float wallDir = 0f;

    Vector3 faceDir;
    // private bools
    bool canAttack;
    bool canShoot;
    bool allowAttack = true;
    bool huggingWall = false;
    bool isGrounded;
    bool doubleJumped = false;
    GameObject hitEnemy;
    GameObject hitEnemyShooting;

    public Vector3 movement;
    CharacterMotor motor;

    void Awake () {
        motor = GetComponent<CharacterMotor> ();
        targetCount = 0;
    }

    void Update()
    {
        animator.SetFloat("InputX", InputX, locoST, Time.deltaTime);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("Attacking", canAttack);
        animator.SetBool("huggingWall", huggingWall);
        animator.SetBool("Shooting", canShoot);

        targetCount_ = targetCount;
        score.text = targetCount.ToString(); // UI-element
        InputX = Input.GetAxisRaw(horizontalAxis);
        InputY = Input.GetAxisRaw("J_MainHorizontal");
        motor.SetVelocity(movement * speed * Time.deltaTime);
        movement = new Vector2(InputX, 0);
        isGrounded = motor.Grounded();

        if (allowAttack)
        {
            canAttack = Input.GetButton(attackAxis);
            canShoot = Input.GetButton(shootAxis);
        }

        if (movement.x != 0)
        {
            faceDir = movement;
        }

        hitEnemy = motor.CheckLayerObstacle(faceDir, attackwidth, 1 << LayerMask.NameToLayer("EnemyLayer"));
        hitEnemyShooting = motor.CheckLayerObstacle(faceDir, shootWidth, (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("EnemyLayer")));

        if (hitEnemy != null)
            Debug.Log(hitEnemy.name);
        if(hitEnemyShooting != null)
            Debug.Log(hitEnemyShooting.name + " shoot can see this");

        if (Input.GetButtonDown (jumpAxis))
        {
            Jumpy();
            stickTimer = 0f;
            exitWallTimer = 0f;
            huggingWall = false;
        }

        if (Input.GetButtonDown (attackAxis))
        {
            Attack();
        }

        if (Input.GetButtonDown(shootAxis))
        {
            Shoot();
        }

        motor.FreezeXAxis (huggingWall);
        motor.FreezeYAxis (huggingWall);

        bool hitWall = motor.CheckObstacle (movement, skinWidth);

        if (hitWall)
        {
            wallDir = Mathf.Sign (movement.x);
            huggingWall = true;
            exitWallTimer = 0f;
        } else if (wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign (movement.x)))
        {
            exitWallTimer += Time.deltaTime;
            if (exitWallTimer > 0.4f)
            {
                huggingWall = false;
                exitWallTimer = 0f;
            }
        } else
        {
            exitWallTimer = 0f;
        }

        if (huggingWall)
        {
            stickTimer += Time.deltaTime;
            if (stickTimer > .6f) {
                huggingWall = false;
            }
        }
    }
    void Jumpy ()
    {
        if (isGrounded || !doubleJumped)
        {
            print (transform.name + " is jumping!");
            motor.ResetPhysics ();
            motor.ApplyForce (new Vector2 (0, jumpForce));
            doubleJumped = !isGrounded;
        }
        else if (huggingWall && wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign (movement.x)))
        {
            motor.ResetPhysics ();
            motor.ApplyForce (new Vector2 (-wallDir * wallJumpForce, jumpForce));
            huggingWall = false;
        }
    }

    void Attack ()
    {
        if (hitEnemy != null)
        {
            if (!huggingWall && canAttack && hitEnemy.transform.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
            {
                Debug.Log("Has attacked");
                hitEnemy.SetActive(false);
                targetCount++;
                allowAttack = false;
                StartCoroutine(ResetAttack());
            }
            StartCoroutine(ResetAttack());
        }
    }

    void Shoot()
    {
        if(hitEnemyShooting != null)
        {
            if (!huggingWall && allowAttack && hitEnemyShooting.transform.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
            {
                Debug.Log("Shot has hit");
                hitEnemyShooting.SetActive(false);
                targetCount++;
                allowAttack = false;
                StartCoroutine(ResetAttack());
            }
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack ()
    {
        yield return new WaitForSeconds (.4f);
        allowAttack = true;
    }
}