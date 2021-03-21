using UnityEngine;
using Cinemachine;
using System;
using ExtensionClass;
using GameLibrary;


namespace GameLibrary
{

    public class PlayerMovement : MonoBehaviour
    {
        [InspectorName("Scripts")]
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private PlayerInputs playerInputs;
        [SerializeField] private GameScript gameManagerScript;

        public float MovementSpeed;
        public float RunningMultiplier;
        public float JumpHeight;
        public float SlideForce;
        public float distanceToGround = 0.3f;
        public float groundDrag = 5f;
        public float airDrag = 2f;
        public float airResistance = 0.45f;
        public Vector3 gravity = Vector3.down * 40f;


        private Rigidbody rigidBody;
        private Animator characterAnimator;
        private CapsuleCollider playerCollider;
        private Vector3 playerDirection;
        private bool isGrounded;


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

            if (playerInputs.IsJumping && isGrounded)
                rigidBody.Jump(JumpHeight);
            else if (playerInputs.IsRunning && playerInputs.IsSlideTriggered && isGrounded && playerInputs.VerticalAxis > 0)
                ProcessSlide();


            gameManagerScript.AddPoints(playerDirection.magnitude * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            isGrounded = rigidBody.IsGrounded(distanceToGround);
            if (isGrounded)
            {
                rigidBody.drag = groundDrag;
                rigidBody.AddForce(playerDirection.normalized * MovementSpeed * (playerInputs.IsRunning ? RunningMultiplier : 1f), ForceMode.Acceleration);
            }
            else
            {
                rigidBody.drag = airDrag;
                rigidBody.AddForce(playerDirection.normalized * MovementSpeed * airResistance * (playerInputs.IsRunning ? RunningMultiplier : 1f) + gravity, ForceMode.Acceleration);
            }
        }


        private void OnCollisionEnter(Collision collision)
        {

        }
        private void OnCollisionExit(Collision collision)
        {

        }



        private void UpdateAnimatorVariables()
        {
            characterAnimator.SetFloat("Speed", playerInputs.VerticalAxis * (playerInputs.IsRunning ? 2 : 1));
            characterAnimator.SetFloat("StrafeSpeed", playerInputs.HorizontalAxis * (playerInputs.IsRunning ? 2 : 1));
            characterAnimator.SetBool("IsJumping", playerInputs.IsJumping);
            characterAnimator.SetBool("IsGrounded", isGrounded);
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

            // Making the player collider smaller when sliding
            var newCenter = playerCollider.center;
            newCenter.y = playerInputs.IsSliding ? 0.3f : 0.9f;
            playerCollider.center = newCenter;
            playerCollider.height = newCenter.y * 2;
        }
    }

}