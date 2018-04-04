using UnityEngine;

public class PlayerMotor : MonoBehaviour {
    private Vector3 moveVector;
    private Vector3 lastMove;
    private float speed = 8f;
    private float jumpForce = 8f;
    private float gravity = 9f;
    private float verticalVelocity;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            verticalVelocity = -1;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveVector.y = 0;
        moveVector.Normalize();
        moveVector *= speed;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
        //lastMove = moveVector;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.normal.y < .1f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.cyan, 1.25f);
                verticalVelocity = jumpForce;
                moveVector = hit.normal * speed;
            }
        }
    }
}