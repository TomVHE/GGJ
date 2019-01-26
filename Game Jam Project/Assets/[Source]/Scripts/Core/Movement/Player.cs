namespace Core.Movement
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using KinematicCharacterController;
    
    public class Player : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private MovementMethod movementMethod = MovementMethod.CharacterForwardIsForward;
        
        [SerializeField] private RotationMethod rotationMethod = RotationMethod.VelocityRotation;

        [SerializeField] private UberCharacterController characterController;
        //public float mouseSensitivity = 0.01f;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private void Start()
        {
            if (mainCamera == null) { mainCamera = Camera.main; }
        }

        private void Update()
        {
            HandleCharacterInput();
        }

        private void HandleCharacterInput()
        {                        
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs()
            {
                MoveAxisForward = Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = Input.GetAxisRaw(HorizontalInput),
                CameraRotation = mainCamera.transform.rotation,
                MovementMethod = movementMethod,
                RotationMethod = rotationMethod
            };

            characterController.SetInputs(ref characterInputs);
        }
    }
}