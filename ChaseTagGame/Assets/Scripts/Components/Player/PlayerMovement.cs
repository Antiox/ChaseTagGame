using UnityEngine;
using Cinemachine;
using System;
using GameLibrary;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary
{

    public class PlayerMovement : MonoBehaviour
    {
        #region Variables

        [Header("Scripts")]
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private PlayerInputs playerInputs;
        [SerializeField] private GameScript gameManagerScript;

        [Header("Movements")]
        [SerializeField] private float MovementSpeed;
        [SerializeField] private float RunningMultiplier;
        [SerializeField] private float SlideForce;
        [SerializeField] private float MaxSlopeAngle = 50f;
        private float slopeForce = 50f;
        private bool canWalkOnSlope;
        private float CurrentRunningSpeed { get { return playerInputs.IsRunning ? RunningMultiplier : 1f; } }
        private Vector3 playerMovement { get { return playerDirection * MovementSpeed; } }


        [Header("Jump")]
        [SerializeField] private float JumpHeight;
        [SerializeField] private float groundDrag;
        [SerializeField] private float airDrag;
        [SerializeField] private float airResistance;
        [SerializeField] private Vector3 gravity;
        private float minAirTime = 0.2f;
        private float currentAirTime;
        private bool isGrounded;
        public bool IsBumped { get; set; }


        [Header("Ground Detection")]
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float groundCheckOffset;
        private Slope currentSlope;

        [Header("Climbing")]
        private Transform faceCaster;
        private bool isAgainstWall;
        private bool isClimbing;
        private bool previousClimbingState;
        private bool reachedTop;
        private bool climbEndInProgress;
        private float climbEndTimer;
        private float maxClimbTime = 15f;
        private float climbingTimer;
        private Vector3 climbEndDestination;
        private Vector3 frontWallNormal;

        private Rigidbody rigidBody;
        private Animator characterAnimator;
        private CapsuleCollider playerCollider;
        private Vector3 playerDirection;
        private Vector3 lastPlayerDirection;

        #endregion

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            characterAnimator = GetComponentInChildren<Animator>();
            playerCollider = GetComponent<CapsuleCollider>();
            gameManagerScript = GameObject.FindGameObjectWithTag(GameTags.GameManager).GetComponent<GameScript>();
            playerCamera = GetComponent<PlayerCamera>();
            playerInputs = GetComponent<PlayerInputs>();
            faceCaster = GameObject.Find("FaceLevelDetector").GetComponent<Transform>();
        }

        void Update()
        {
            FaceCameraDirection();
            UpdateAnimatorVariables();


            if (playerInputs.IsJumping && isGrounded && canWalkOnSlope)
                ProcessJump();
            else if (playerInputs.IsRunning && playerInputs.IsSlideTriggered && isGrounded && playerInputs.VerticalAxis > 0)
                ProcessSlide();

            if (reachedTop)
            {
                climbEndDestination = transform.position + transform.forward *0.6f + transform.up * 1.6f;
                playerCollider.enabled = false;
            }
            if (climbEndInProgress)
                ProcessEndOfClimb();


            lastPlayerDirection = playerDirection.magnitude > 0 ? playerDirection : lastPlayerDirection;
            climbEndTimer += climbEndInProgress ? Time.deltaTime : 0f;
            climbingTimer += isClimbing ? (Mathf.Sign(rigidBody.velocity.y) * rigidBody.velocity.magnitude + 1)  * Time.deltaTime : 0f;
            currentAirTime = isGrounded ? 0 : (currentAirTime + Time.deltaTime);
            climbEndInProgress = reachedTop ? true : climbEndInProgress;
            previousClimbingState = isClimbing;
            isClimbing = isAgainstWall && !isGrounded && rigidBody.velocity.magnitude > 0 && playerInputs.IsHoldingJump && climbingTimer <= maxClimbTime;
            reachedTop = previousClimbingState && !isClimbing && !isGrounded && playerInputs.IsHoldingJump && climbingTimer <= maxClimbTime;
            rigidBody.drag = GetCurrentDrag();

            if (isGrounded)
                climbingTimer = 0;

            if (transform.position == climbEndDestination)
            {
                climbEndInProgress = false;
                playerCollider.enabled = true;
                reachedTop = false;
                climbEndTimer = 0;
            }

            EventManager.Instance.Broadcast(new OnPointsAddedEvent(playerDirection.magnitude * Time.deltaTime));
        }

        private void FixedUpdate()
        {
            currentSlope = rigidBody.GetSlope(lastPlayerDirection);
            isGrounded = rigidBody.IsGrounded(groundCheckRadius) && playerCollider.enabled;
            canWalkOnSlope = rigidBody.CanWalkOnSlope(currentSlope, MaxSlopeAngle);
            isAgainstWall = rigidBody.IsFacingWall(faceCaster);
            frontWallNormal = isAgainstWall ? rigidBody.GetFacingWallNormal(faceCaster) : Vector3.zero;

            var frameForce = DetermineFinalFrameForce();
            if (frameForce.magnitude > 0)
                rigidBody.AddForce(frameForce, ForceMode.Acceleration);
        }

        private void UpdateAnimatorVariables()
        {
            characterAnimator.SetFloat("Speed", playerInputs.VerticalAxis * (playerInputs.IsRunning ? 2 : 1));
            characterAnimator.SetFloat("StrafeSpeed", playerInputs.HorizontalAxis * (playerInputs.IsRunning ? 2 : 1));
            characterAnimator.SetFloat("ClimbingSpeed", Mathf.Clamp(rigidBody.velocity.y, -10, 5));
            characterAnimator.SetBool("IsJumping", playerInputs.IsJumping && canWalkOnSlope);
            characterAnimator.SetBool("IsHoldingJump", playerInputs.IsHoldingJump);
            characterAnimator.SetBool("IsGrounded", currentAirTime <= minAirTime);
            characterAnimator.SetBool("IsSliding", playerInputs.IsSliding);
            characterAnimator.SetBool("IsBeginningSliding", playerInputs.IsSlideTriggered);
            characterAnimator.SetBool("IsClimbing", isClimbing);
            characterAnimator.SetBool("ClimbedMaxTime", climbingTimer >= maxClimbTime);
        }
        private void FaceCameraDirection()
        {
            var cameraFrontDirection = playerCamera.CameraTargetTransform.position - playerCamera.CameraTransform.position;
            cameraFrontDirection.y = 0;
            cameraFrontDirection = cameraFrontDirection.normalized;
            var cameraSideDirection = Quaternion.AngleAxis(90, Vector3.up) * cameraFrontDirection;
            playerDirection = (cameraFrontDirection * playerInputs.VerticalAxis + cameraSideDirection * playerInputs.HorizontalAxis).normalized;

            if (playerDirection.magnitude > 0 && !isClimbing && !climbEndInProgress)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraFrontDirection), 25f * Time.deltaTime);
            else if(isClimbing)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-frontWallNormal), 25f * Time.deltaTime);
        }
        private void ProcessSlide()
        {
            rigidBody.Slide(SlideForce);
            ReduceColliderWhenSliding();
        }
        private void ProcessJump()
        {
            rigidBody.Jump(JumpHeight);
        }
        private float GetCurrentDrag()
        {
            if (IsBumped)
                return airDrag;
            if (!isGrounded)
                return airDrag;
            else if (isGrounded && !playerInputs.IsMoving)
                return groundDrag * 4f;
            else if (isGrounded && playerInputs.IsSliding)
                return groundDrag / 3f;
            else if (isGrounded && playerInputs.IsMoving)
                return groundDrag;

            return 2f;
        }
        private void ReduceColliderWhenSliding()
        {
            var newCenter = playerCollider.center;
            newCenter.y = playerInputs.IsSliding ? 0.3f : 0.9f;
            playerCollider.center = newCenter;
            playerCollider.height = newCenter.y * 2;
        }
        private Vector3 DetermineFinalFrameForce()
        {
            var inAirPlayerMovement = playerMovement * airResistance + gravity;
            var groundPlayerMovement = canWalkOnSlope ? (playerMovement * CurrentRunningSpeed) : playerMovement / 2f;
            var slopeDownForce = (canWalkOnSlope ? -currentSlope.Normal : Vector3.down) * slopeForce;
            var climbingForce = isClimbing ? Vector3.up * 8f * (maxClimbTime - climbingTimer) / maxClimbTime : Vector3.zero;
            var frameForce =Vector3.zero;

            if (isGrounded)
                frameForce += groundPlayerMovement;
            if(!isGrounded)
                frameForce += inAirPlayerMovement;
            if (currentSlope.IsOnSlope)
                frameForce += slopeDownForce;
            if (isClimbing)
                frameForce = climbingForce;
            if (climbEndInProgress)
            {
                frameForce = Vector3.zero;
                rigidBody.velocity = Vector3.zero;
            }

            return frameForce;
        }
        private void ProcessEndOfClimb()
        {
            transform.position = Vector3.Lerp(transform.position, climbEndDestination, climbEndTimer / 2f);
        }
    }
}