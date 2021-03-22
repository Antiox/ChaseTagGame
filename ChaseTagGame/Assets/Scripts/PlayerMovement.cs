using UnityEngine;
using Cinemachine;
using System;
using ExtensionClass;
using GameLibrary;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary
{

    public class PlayerMovement : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private PlayerInputs playerInputs;
        [SerializeField] private GameScript gameManagerScript;

        [Header("Movements")]
        [SerializeField] private float MovementSpeed;
        [SerializeField] private float RunningMultiplier;
        [SerializeField] private float SlideForce;
        [SerializeField] private float MaxSlopeAngle = 50f;

        [Header("Jump")]
        [SerializeField] private float JumpHeight;
        [SerializeField] private float groundDrag;
        [SerializeField] private float airDrag;
        [SerializeField] private float airResistance;
        [SerializeField] private Vector3 gravity;

        [Header("Ground Detection")]
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float groundCheckOffset;


        private Rigidbody rigidBody;
        private Animator characterAnimator;
        private CapsuleCollider playerCollider;
        private Vector3 playerDirection;
        private Vector3 lastPlayerDirection;
        private bool isGrounded;
        private float slopeForce = 50f;
        private float minAirTime = 0.2f;
        private float currentAirTime;
        private bool canWalkOnSlope;
        private Vector3 playerMovement { get { return playerDirection * MovementSpeed; } }
        private float CurrentRunningSpeed { get { return playerInputs.IsRunning ? RunningMultiplier : 1f; } }


        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            characterAnimator = GetComponentInChildren<Animator>();
            playerCollider = GetComponent<CapsuleCollider>();
            gameManagerScript = GameObject.FindGameObjectWithTag(GameTags.GameManager).GetComponent<GameScript>();
            playerCamera = GetComponent<PlayerCamera>();
            playerInputs = GetComponent<PlayerInputs>();
        }

        void Update()
        {
            FaceCameraDirection();
            UpdateAnimatorVariables();

            if (playerInputs.IsJumping && isGrounded && canWalkOnSlope)
                rigidBody.Jump(JumpHeight);
            else if (playerInputs.IsRunning && playerInputs.IsSlideTriggered && isGrounded && playerInputs.VerticalAxis > 0)
                ProcessSlide();


            gameManagerScript.AddPoints(playerDirection.magnitude * Time.deltaTime);

            if (playerDirection.magnitude > 0)
                lastPlayerDirection = playerDirection;
        }

        private void FixedUpdate()
        {
            var slope = rigidBody.GetSlope(lastPlayerDirection);
            isGrounded = rigidBody.IsGrounded(groundCheckRadius);
            canWalkOnSlope = rigidBody.CanWalkOnSlope(slope, MaxSlopeAngle);
            currentAirTime = isGrounded ? 0 : (currentAirTime + Time.deltaTime);
            rigidBody.drag = isGrounded ? groundDrag * (playerInputs.IsMoving ? 1f : 4f) : airDrag;

            var inAirPlayerMovement = playerMovement * airResistance * CurrentRunningSpeed + gravity;
            var groundPlayerMovement = canWalkOnSlope ? (playerMovement * CurrentRunningSpeed) : playerMovement * CurrentRunningSpeed / 2f;

            var slopeDownForce = (canWalkOnSlope ? -slope.Normal : Vector3.down) * slopeForce;
            var frameForce = (isGrounded ? groundPlayerMovement : inAirPlayerMovement) + slopeDownForce;

            if (frameForce.magnitude > 0)
                rigidBody.AddForce(frameForce, ForceMode.Acceleration);
        }


        private void UpdateAnimatorVariables()
        {
            characterAnimator.SetFloat("Speed", playerInputs.VerticalAxis * (playerInputs.IsRunning ? 2 : 1));
            characterAnimator.SetFloat("StrafeSpeed", playerInputs.HorizontalAxis * (playerInputs.IsRunning ? 2 : 1));
            characterAnimator.SetBool("IsJumping", playerInputs.IsJumping && canWalkOnSlope);
            characterAnimator.SetBool("IsGrounded", currentAirTime <= minAirTime);
            characterAnimator.SetBool("IsSliding", playerInputs.IsSliding);
            characterAnimator.SetBool("IsBeginningSliding", playerInputs.IsSlideTriggered);
        }
        private void FaceCameraDirection()
        {
            var cameraFrontDirection = playerCamera.CameraTargetTransform.position - playerCamera.CameraTransform.position;
            cameraFrontDirection.y = 0;
            cameraFrontDirection = cameraFrontDirection.normalized;
            var cameraSideDirection = Quaternion.AngleAxis(90, Vector3.up) * cameraFrontDirection;
            playerDirection = (cameraFrontDirection * playerInputs.VerticalAxis + cameraSideDirection * playerInputs.HorizontalAxis).normalized;

            if (playerDirection.magnitude > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraFrontDirection), 25f * Time.deltaTime);
        }
        private void ProcessSlide()
        {
            rigidBody.Slide(SlideForce);
            ReduceColliderWhenSliding();
        }
        private void ReduceColliderWhenSliding()
        {
            var newCenter = playerCollider.center;
            newCenter.y = playerInputs.IsSliding ? 0.3f : 0.9f;
            playerCollider.center = newCenter;
            playerCollider.height = newCenter.y * 2;
        }
    }
}