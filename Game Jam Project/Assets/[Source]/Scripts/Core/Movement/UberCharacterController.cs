namespace Core.Movement
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using KinematicCharacterController;
    using System;
    
    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward { get; set; }
        public float MoveAxisRight { get; set; }
        public Quaternion CameraRotation { get; set; }
        public MovementMethod MovementMethod { get; set; }
        public RotationMethod RotationMethod { get; set; }
    }

    public struct AICharacterInputs
    {
        public Vector3 MoveVector;
        public Vector3 TargetLookDirection;
    }

    public enum MovementMethod
    {
        CameraIsForward,
        CharacterForwardIsForward,
        StaticForward
    }
    
    public enum RotationMethod
    {
        NoRotation,
        KeyboardRotation,
        MouseJoystick,
        MouseFollowRotation,
        VelocityRotation
    }
    
    public class UberCharacterController : BaseCharacterController
    {
        #region Variables
        
        #region Serialized Variables
        
        [Header("Stable Movement")]
        [SerializeField] private float maxStableMoveSpeed = 10f;
        [SerializeField] private float stableMovementSharpness = 15;
        public float orientationSharpness = 10;

        [Header("Air Movement")]
        [SerializeField] private float maxAirMoveSpeed = 10f;
        [SerializeField] private float airAccelerationSpeed = 5f;
        [SerializeField] private float drag = 0.1f;

        [Header("Misc")]
        [SerializeField] private Vector3 gravity = new Vector3(0, -30f, 0);
        //[SerializeField] private Transform meshRoot;
                
        #endregion

        #region Non-Serialized Variables

        private Vector3 MoveInputVector { get; set; }
        private Vector3 LookInputVector  { get; set; }
        private MovementMethod MethodOfMovement { get; set; }
        private RotationMethod MethodOfRotation { get; set; }

        #endregion
        
        #endregion

        /// <summary>
        /// This is called every frame by Player in order to tell the character what its inputs are.
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            Vector3 _moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            float lookInput = 0;

            if (inputs.RotationMethod == RotationMethod.KeyboardRotation)
            {
                lookInput = Motor.transform.rotation.y + inputs.MoveAxisRight * 35f; //TODO: Replace hardcoding with proper sensitivity value
            }
            else if (inputs.RotationMethod == RotationMethod.MouseJoystick)
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x / Screen.width,Input.mousePosition.y / Screen.height, 0);
                Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0);
                
                lookInput = Mathf.Atan2(mousePosition.y - screenCenter.y, mousePosition.x - screenCenter.x) * (180f/Mathf.PI);
            }
            
            #region Camera-Rotation

            Vector3 _cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (_cameraPlanarDirection.sqrMagnitude == 0f)
            {
                _cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(_cameraPlanarDirection, Motor.CharacterUp);
            
            #endregion
            
            #region Character-Rotation
            
            Quaternion rotationFromInput = Quaternion.Euler(Motor.CharacterUp * lookInput);
            Vector3 _characterPlanarDirection = rotationFromInput * Motor.CharacterForward;
            _characterPlanarDirection = Vector3.Cross(Motor.CharacterUp, Vector3.Cross(_cameraPlanarDirection, Motor.CharacterUp));
            Quaternion characterPlanarRotation = Quaternion.LookRotation(_characterPlanarDirection, Motor.CharacterUp);

            #endregion
            
            //LookInputVector = new Vector3(lookInput, 0, 0f);

            switch (inputs.RotationMethod)
            {
                case (RotationMethod.NoRotation):
                {
                    LookInputVector = _cameraPlanarDirection;
                    break;
                }
                case (RotationMethod.KeyboardRotation):
                {
                    _moveInputVector = new Vector3(0, _moveInputVector.y, _moveInputVector.z);
                    LookInputVector = _characterPlanarDirection;
                    break;
                }
                case (RotationMethod.VelocityRotation):
                {
                    LookInputVector = Motor.BaseVelocity;
                    break;
                }
                case (RotationMethod.MouseJoystick):
                {
                    //_moveInputVector = new Vector3(0, _moveInputVector.y, _moveInputVector.z);
                    LookInputVector = _characterPlanarDirection;
                    break;
                }
            }
            
            switch (inputs.MovementMethod)
            {
                case (MovementMethod.StaticForward):
                {
                    MoveInputVector = _moveInputVector;
                    break;
                }
                case (MovementMethod.CameraIsForward):
                {
                    MoveInputVector = cameraPlanarRotation * _moveInputVector;
                    break;
                }
                case (MovementMethod.CharacterForwardIsForward):
                {
                    MoveInputVector = characterPlanarRotation * _moveInputVector;
                    break;
                }
                default:
                {
                    MoveInputVector = _moveInputVector;
                    break;
                }
            }

            MethodOfMovement = inputs.MovementMethod;
            MethodOfRotation = inputs.RotationMethod;
        }

        /// <summary>
        /// This is called every frame by AI in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref AICharacterInputs inputs)
        {
            // Move and look inputs
            MoveInputVector = inputs.MoveVector;

            if (MoveInputVector.sqrMagnitude == 0f)
            {
                LookInputVector = Motor.CharacterForward;
            }
            else
            {
                LookInputVector = Vector3.ProjectOnPlane(MoveInputVector, Motor.CharacterUp);
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public override void BeforeCharacterUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (LookInputVector != Vector3.zero && orientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, LookInputVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 targetMovementVelocity = Vector3.zero;
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(MoveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * MoveInputVector.magnitude;
                targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-stableMovementSharpness * deltaTime));
            }
            /*
            else
            {
                // Add move input
                if (MoveInputVector.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = MoveInputVector * maxAirMoveSpeed;

                    // Prevent climbing on un-stable slopes with air movement
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, gravity);
                    currentVelocity += velocityDiff * airAccelerationSpeed * deltaTime;
                }

                // Gravity
                currentVelocity += gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (drag * deltaTime)));
            }
            */
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public override void AfterCharacterUpdate(float deltaTime)
        {
        }

        public override bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public override void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
        }

        public override void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }
    }
}