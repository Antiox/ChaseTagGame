using UnityEngine;
using Cinemachine;
using System;
using ExtensionClass;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Animator characterAnimator;
    private CapsuleCollider playerCollider;
    private CinemachineFreeLook freeLookCamera;
    private Vector3 nextMovement;
    private GameScript gameManagerScript;

    public float MoveFactor;
    public float TurnFactor;
    public float RunningFactor;
    public float JumpHeight;
    public float SlideFactor;

    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private bool isSliding;
    private bool isBeginningSliding;
    private float horizontalAxis;
    private float verticalAxis;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponentInChildren<Animator>();
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        playerCollider = GetComponent<CapsuleCollider>();
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameScript>();
    }

    void Update()
    {
        CaptureInputs();
        FollowCameraDirection();
        UpdateAnimatorVariables();

        isGrounded = rigidBody.IsGrounded(playerCollider.bounds.extents.y);

        if (isJumping && isGrounded)
            Jump();
        else if (isRunning && isBeginningSliding && isGrounded && verticalAxis > 0)
            Slide();


        var newCenter = playerCollider.center;
        newCenter.y = isSliding ? 0.3f : 0.9f;
        playerCollider.center = newCenter;
        playerCollider.height = newCenter.y * 2;

        gameManagerScript.AddPoints(nextMovement.magnitude * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + nextMovement * MoveFactor * Time.fixedDeltaTime * (isRunning ? RunningFactor : 1f));
        rigidBody.IncreaseGravity(1.5f);
    }




    private void CaptureInputs()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        isRunning = Input.GetButton("Run");
        isJumping = Input.GetButtonDown("Jump");
        isSliding = Input.GetButton("Slide") && !Extensions.ApproximatelyEquals(rigidBody.velocity.magnitude, 0);
        isBeginningSliding = Input.GetButtonDown("Slide");
    }
    private void UpdateAnimatorVariables()
    {
        characterAnimator.SetFloat("Speed", verticalAxis * (isRunning ? 2 : 1));
        characterAnimator.SetFloat("StrafeSpeed", horizontalAxis * (isRunning ? 2 : 1));
        characterAnimator.SetBool("IsJumping", isJumping);
        characterAnimator.SetBool("IsGrounded", rigidBody.IsGrounded(playerCollider.bounds.extents.y));
        characterAnimator.SetBool("IsSliding", isSliding);
        characterAnimator.SetBool("IsBeginningSliding", isBeginningSliding);
    }
    private void FollowCameraDirection()
    {
        var cameraFrontDirection = freeLookCamera.LookAt.position - freeLookCamera.transform.position;
        cameraFrontDirection.y = 0;
        cameraFrontDirection = cameraFrontDirection.normalized;
        var cameraSideDirection = Quaternion.AngleAxis(90, Vector3.up) * cameraFrontDirection;
        var forwardMovement = new Vector3(verticalAxis * cameraFrontDirection.x, 0, verticalAxis * cameraFrontDirection.z);
        var sideMovement = new Vector3(horizontalAxis * cameraSideDirection.x, 0, horizontalAxis * cameraSideDirection.z);

        if ((isSliding || isJumping) && Extensions.ApproximatelyEquals(verticalAxis, 0))
            sideMovement = Vector3.zero;

        nextMovement = (sideMovement + forwardMovement).normalized;


        if (nextMovement.magnitude > 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraFrontDirection), TurnFactor * Time.deltaTime);
    }
    private void Jump()
    {
        rigidBody.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -1f * Physics.gravity.y), ForceMode.VelocityChange);
    }
    private void Slide()
    {
        rigidBody.AddForce(nextMovement.normalized * SlideFactor, ForceMode.VelocityChange);
    }
}
