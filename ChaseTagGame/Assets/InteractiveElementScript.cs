using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameLibrary
{
    public class InteractiveElementScript : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltip;
        [SerializeField] private ActionType action;
        [SerializeField] private string tooltipText;
        [SerializeField, Range(0f, 1f)] private float tolerance;
        [SerializeField, Range(0f, 5f)] private float minDistance;
        [SerializeField] private float expandSpeed = 16f;
        private float expandDistance = 0.18f;

        private Camera mainCamera;
        private GameObject player;
        private bool playerIsPressingAction;
        private bool buttonIsActivated;
        private Vector3 buttonInitialPosition;
        private Vector3 buttonTargetPosition;



        private void Start()
        {
            mainCamera = Camera.main;
            player = GameObject.FindGameObjectWithTag(GameTags.Player);
            buttonInitialPosition = transform.position;
            buttonTargetPosition = transform.position - (transform.forward * expandDistance);
        }

        private void Update()
        {
            playerIsPressingAction = Input.GetButtonDown("Action");

            if (buttonIsActivated && playerIsPressingAction)
            {
                var e = new OnInteractiveElementPressedEvent(action);
                EventManager.Instance.Dispatch(e);
                StartCoroutine(AnimateButton(buttonInitialPosition, buttonTargetPosition));
            }
        }

        private void OnTriggerStay(Collider other)
        {
            buttonIsActivated = other.CompareTag(GameTags.Player) && IsPlayerFacingButton() && GameManager.State == GameState.InGame;
            tooltip.enabled = buttonIsActivated;
            tooltip.text = tooltipText;
        }

        private void OnTriggerExit(Collider other)
        {
            tooltip.enabled = false;
        }



        private IEnumerator AnimateButton(Vector3 initialPosition, Vector3 targetPosition)
        {
            float t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime * expandSpeed;
                transform.position = (Vector3.Lerp(initialPosition, targetPosition, t));

                yield return new WaitForEndOfFrame();
            }

            t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime * expandSpeed;
                transform.position = (Vector3.Lerp(targetPosition, initialPosition, t));

                yield return new WaitForEndOfFrame();
            }
        }

        private bool IsPlayerFacingButton()
        {
            var cameraDirection = mainCamera.transform.forward;
            var playerPosition = player.transform.position + (Vector3.up * 1f);
            var buttonDirection = (transform.position - playerPosition).normalized;
            var angle = Vector3.Dot(cameraDirection, buttonDirection);
            var distance = Vector3.Distance(playerPosition, transform.position);

            return angle >= tolerance || distance <= minDistance;
        }
    }
}