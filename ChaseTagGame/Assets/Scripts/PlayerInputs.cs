using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerInputs : MonoBehaviour
    {
        public float HorizontalAxis { get; private set; }
        public float VerticalAxis { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsHoldingJump { get; private set; }
        public bool IsSlideTriggered { get; private set; }
        public bool IsSliding { get; private set; }
        public bool IsMoving { get; private set; }


        void Update()
        {
            HorizontalAxis = Input.GetAxisRaw("Horizontal");
            VerticalAxis = Input.GetAxisRaw("Vertical");
            IsRunning = Input.GetButton("Run");
            IsJumping = Input.GetButtonDown("Jump");
            IsHoldingJump = Input.GetButton("Jump");
            IsSliding = Input.GetButton("Slide");
            IsSlideTriggered = Input.GetButtonDown("Slide");
            IsMoving = HorizontalAxis != 0 || VerticalAxis != 0;
        }
    }
}
