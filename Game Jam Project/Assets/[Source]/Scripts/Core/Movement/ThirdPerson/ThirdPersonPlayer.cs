namespace Core.Movement
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using KinematicCharacterController;
    
    public class ThirdPersonPlayer : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ThirdPersonCharacterController characterController;

        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private void Start()
        {
            if (characterController == null) { characterController = GetComponent<ThirdPersonCharacterController>();}
            if (mainCamera == null) { mainCamera = Camera.main; }
        }

        private void Update()
        {
            HandleCharacterInput();
        }
        
        private void HandleCharacterInput()
        {                        
            TPPlayerCharacterInputs characterInputs = new TPPlayerCharacterInputs()
            {
                MoveAxisForward = Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = Input.GetAxisRaw(HorizontalInput),
                CameraRotation = mainCamera.transform.rotation,
            };

            characterController.SetInputs(ref characterInputs);
        }
    }
}