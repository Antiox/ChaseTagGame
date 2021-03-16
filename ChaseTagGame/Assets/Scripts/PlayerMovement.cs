using UnityEngine;
using Cinemachine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Animator characterAnimator;
    private CinemachineFreeLook freeLookCamera;

    public float MoveFactor;
    public float TurnFactor;
    public float RunningFactor;
    public float JumpHeight;
    private Vector3 movement;
    private bool isRunning;
    private bool isJumping;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponentInChildren<Animator>();
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        isRunning = Input.GetButton("Run");
        isJumping = Input.GetButtonDown("Jump");


        var cameraDirection = freeLookCamera.LookAt.position - freeLookCamera.transform.position;
        cameraDirection.y = 0;
        cameraDirection = cameraDirection.normalized;
        var strafeDirection  = Quaternion.AngleAxis(90, Vector3.up) * cameraDirection;

        movement = new Vector3(vertical * cameraDirection.x, 0, vertical * cameraDirection.z);
        movement = movement + new Vector3(horizontal * strafeDirection.x, 0, horizontal * strafeDirection.z);
        movement = movement.normalized;

        if (movement.magnitude > 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraDirection), TurnFactor * Time.deltaTime);


        if (isJumping)
            rigidBody.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -1f * Physics.gravity.y), ForceMode.VelocityChange);

        if (Math.Abs(rigidBody.velocity.y) > 0.001)
        {
            var velocity = rigidBody.velocity;
            velocity.y += Physics.gravity.y * 1.5f * Time.deltaTime;
            rigidBody.velocity = velocity;
        }

        Debug.Log((1f / Time.unscaledDeltaTime));
        characterAnimator.SetFloat("Speed", vertical * (isRunning ? 2 : 1));
        characterAnimator.SetFloat("StrafeSpeed", horizontal * (isRunning ? 2 : 1));
        characterAnimator.SetBool("IsJumping", isJumping);

    }

    private void LateUpdate()
    {
        characterAnimator.SetBool("IsInTheAir", Math.Abs(rigidBody.velocity.y) > 0.001);
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement * MoveFactor * Time.fixedDeltaTime * (isRunning ? RunningFactor : 1f));
    }
}
