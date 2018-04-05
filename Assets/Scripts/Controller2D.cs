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

    public List<GameObject> nearTarget = new List<GameObject> ();
    static public int targetCount = 0;

    LayerMask EnemyLayer;
    [SerializeField]
    string jumpAxis;
    [SerializeField]
    string horizontalAxis;
    //[SerializeField]
    //string verticalAxis = "J_MainVertical";
    [SerializeField]
    string attackAxis;

    // private vars
    float InputX = 0f;
    float stickTimer = 0f;
    float exitTimer = 0f;
    float skinWidth = 0.03f;
    float attackwidth = 1.5f;
    float wallDir = 0f;

    Vector3 faceDir;

    // private bools
    bool canAttack = true;
    bool huggingWall = false;
    bool isGrounded;
    bool doubleJumped = false;
    GameObject hitEnemy;

    Vector3 movement;
    CharacterMotor motor;

    void Awake () {
        motor = GetComponent<CharacterMotor> ();
    }

    void Update () {
        score.text = targetCount.ToString (); // UI-element
        
        InputX = Input.GetAxisRaw (horizontalAxis);
        motor.SetVelocity (movement * speed * Time.deltaTime);
        isGrounded = motor.Grounded ();
        movement = new Vector2 (InputX, 0);

        if (movement.x != 0)
        {
            faceDir = movement;
        }

        hitEnemy = motor.CheckLayerObstacle(faceDir, attackwidth, 1 << LayerMask.NameToLayer("EnemyLayer"));
        if (hitEnemy != null)
            Debug.Log(hitEnemy.name);

        if (Input.GetButtonDown (jumpAxis)) {
            Jumpy ();
            stickTimer = 0f;
            exitTimer = 0f;
            huggingWall = false;
        }

        if (Input.GetButtonDown (attackAxis)) {
            Attack ();
            canAttack = false;
        }

        motor.FreezeXAxis (huggingWall);
        motor.FreezeYAxis (huggingWall);

        bool hitWall = motor.CheckObstacle (movement, skinWidth);

        if (hitWall) {
            wallDir = Mathf.Sign (movement.x);
            huggingWall = true;
            exitTimer = 0f;
        } else if (wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign (movement.x))) {
            exitTimer += Time.deltaTime;
            if (exitTimer > 0.5f) {
                huggingWall = false;
                exitTimer = 0f;
            }
        } else {
            exitTimer = 0f;
        }

        if (huggingWall) {
            stickTimer += Time.deltaTime;
            if (stickTimer > 1f) {
                huggingWall = false;
            }
        }
    }
    void Jumpy () {
        if (isGrounded || !doubleJumped) {
            print (transform.name + " is jumping!");
            motor.ResetPhysics ();
            motor.ApplyForce (new Vector2 (0, jumpForce));
            doubleJumped = !isGrounded;
        }
        else if (huggingWall && wallDir == ((movement.x == 0) ? 0 : -Mathf.Sign (movement.x))) {
            motor.ResetPhysics ();
            motor.ApplyForce (new Vector2 (-wallDir * wallJumpForce, jumpForce));
            huggingWall = false;
        }
    }

    void Attack () {
        if (canAttack && hitEnemy) {
            Debug.Log ("Has attacked");
            StartCoroutine (ResetAttack ());
        }
        StartCoroutine (ResetAttack ());
    }

    IEnumerator ResetAttack () {
        yield return new WaitForSeconds (.4f);
        canAttack = true;
    }
}