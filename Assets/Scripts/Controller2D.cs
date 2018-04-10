using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Controller2D : MonoBehaviour {
#region vars
    // public vars
    public float speed = 400f;
    public float jumpForce = 30f;
    public float dashForce = 20f;
    public float wallJumpForce = 5f;
    float projectileSpeed = 10f;
    public Animator animator;
    public Text score;
    public Rigidbody2D prefab;
    Vector3 projectileSpawn = new Vector3(0,0.9f,0);

    public AudioClip attackClip;
    public AudioClip shootHitClip;
    public AudioClip shootClip;

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
    [SerializeField]
    string left1Axis;
    [SerializeField]
    string right1Axis;

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
    CharacterMotor motor;
    AudioSource aSource;
    public Vector3 movement;

    #endregion
    void Awake () {
        motor = GetComponent<CharacterMotor>();
        aSource = GetComponent<AudioSource>();
        aSource.playOnAwake = false;
        aSource.clip = attackClip;
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

#region buttonpresses
        if (Input.GetButtonDown (attackAxis))
        {
            Attack();
        }

        if (Input.GetButtonDown(shootAxis))
        {
            Shoot();
        }

        if (Input.GetButtonDown(left1Axis))
        {
            DashLeft();
        }

        if (Input.GetButtonDown(right1Axis))
        {
            DashRight();
        }
#endregion
        if(!motor.CheckLayerObstacle(faceDir, attackwidth, 1 << LayerMask.NameToLayer("Bounds")))
        {
            motor.FreezeXAxis(huggingWall);
            motor.FreezeYAxis(huggingWall);
        }

        bool hitWall = motor.CheckObstacle (movement, skinWidth);

        if (hitWall)
        {
            wallDir = Mathf.Sign (movement.x);
            huggingWall = true;
            exitWallTimer = 0f;
        } else if (wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign (movement.x)))
        {
            exitWallTimer += Time.deltaTime;
            if (exitWallTimer > 1.0f)
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

#region Functies
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

    void DashRight()
    {
        motor.ResetPhysics();
        motor.ApplyForce(new Vector2(dashForce, 0));
    }

    void DashLeft()
    {
        motor.ResetPhysics();
        motor.ApplyForce(new Vector2(-dashForce, 0));
    }

    void Attack ()
    {
        if (hitEnemy != null)
        {
            if (!huggingWall && canAttack && hitEnemy.transform.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
            {
                Debug.Log("Has attacked");
                hitEnemy.SetActive(false);
                aSource.clip = attackClip;
                aSource.Play();
                targetCount++;
                allowAttack = false;
                StartCoroutine(ResetAttack());
            }
            StartCoroutine(ResetAttack());
        }
    }

    void Shoot()
    {
        if (allowAttack)
        {
            Rigidbody2D projectile;
            projectile = Instantiate(prefab, (projectileSpawn + faceDir) + transform.position, Quaternion.identity) as Rigidbody2D;
            allowAttack = false;
            aSource.clip = shootClip; //Shoot sound.
            aSource.Play();
            StartCoroutine(ResetAttack());

            if (faceDir.x > 0)
            {
                projectile.AddRelativeForce(new Vector2(projectileSpeed, 0));
            }
            else
            {
                projectile.AddRelativeForce(new Vector2(-projectileSpeed, 0));
            }

            if (hitEnemyShooting != null)
            {
                if (!huggingWall && hitEnemyShooting.transform.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
                {
                    Debug.Log("Shot has hit");
                    aSource.clip = shootHitClip; //Shot hit sound.
                    aSource.Play();
                    hitEnemyShooting.SetActive(false);
                    targetCount++;
                    allowAttack = false;
                    StartCoroutine(ResetAttack());
                }
                StartCoroutine(ResetAttack());
            }
        }
    }

    IEnumerator ResetAttack ()
    {
        yield return new WaitForSeconds (.4f);
        allowAttack = true;
    }
#endregion
}