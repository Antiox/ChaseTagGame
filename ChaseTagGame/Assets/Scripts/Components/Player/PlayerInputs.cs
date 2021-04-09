using System;
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
        public bool IsPressingActionButton { get; private set; }
        public bool CanClimb { get { return canClimb; } }

        private static bool canJump;
        private static bool canSlide;
        private static bool canRun;
        private static bool canClimb;

        void Start()
        {
            EventManager.Instance.AddListener<OnGameOverEvent>(OnGameOver);
            EventManager.Instance.AddListener<OnSkillBoughtEvent>(OnSkillBought);
            EventManager.Instance.AddListener<OnDayEndedEvent>(OnDayEnded);
        }

        void Update()
        {
            if(GameManager.State != GameState.GameOver && GameManager.State != GameState.Shopping)
            {
                HorizontalAxis = Input.GetAxisRaw("Horizontal");
                VerticalAxis = Input.GetAxisRaw("Vertical");
                IsRunning = Input.GetButton("Run")&& canRun;
                IsJumping = Input.GetButtonDown("Jump") && canJump;
                IsHoldingJump = Input.GetButton("Jump");
                IsSliding = Input.GetButton("Slide") && canSlide;
                IsSlideTriggered = Input.GetButtonDown("Slide") && canSlide;
                IsMoving = HorizontalAxis != 0 || VerticalAxis != 0;
            }
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnGameOverEvent>(OnGameOver);
            EventManager.Instance.RemoveListener<OnDayEndedEvent>(OnDayEnded);
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
        }



        private void LockInputs()
        {
            HorizontalAxis = 0;
            VerticalAxis = 0;
            IsRunning = false;
            IsJumping = false;
            IsHoldingJump = false;
            IsSliding = false;
            IsSlideTriggered = false;
            IsMoving = HorizontalAxis != 0 || VerticalAxis != 0;
            canJump = false;
            canSlide = false;
            canRun = false;
            canClimb = false;
        }



        private void OnGameOver(OnGameOverEvent e)
        {
            LockInputs();
        }

        private void OnDayEnded(OnDayEndedEvent e)
        {
            LockInputs();
        }

        private void OnSkillBought(OnSkillBoughtEvent e)
        {
            switch (e.Skill.type)
            {
                case SkillType.Jump: canJump = true; break;
                case SkillType.Climb: canClimb = true; break;
                case SkillType.Slide: canSlide = true; break;
                case SkillType.Run: canRun = true; break;
            }
        }
    }
}
