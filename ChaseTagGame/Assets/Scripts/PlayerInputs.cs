using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerInputs : MonoBehaviour
    {
        public float HorizontalAxis { get; set; }
        public float VerticalAxis { get; set; }
        public bool IsRunning { get; set; }
        public bool IsJumping { get; set; }
        public bool IsSlideTriggered { get; set; }
        public bool IsSliding { get; set; }


        void Update()
        {
            HorizontalAxis = Input.GetAxisRaw("Horizontal");
            VerticalAxis = Input.GetAxisRaw("Vertical");
            IsRunning = Input.GetButton("Run");
            IsJumping = Input.GetButtonDown("Jump");
            IsSliding = Input.GetButton("Slide");
            IsSlideTriggered = Input.GetButtonDown("Slide");
        }
    }
}
