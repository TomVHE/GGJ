namespace Core.Movement
{
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using Sirenix.OdinInspector;
	using System.Linq;
	using Core.Utilities;

	[RequireComponent(typeof(Rigidbody), typeof(VehicleInput))]
	public class VehicleMovement : MonoBehaviour
	{
		#region Variables

		#region Serialized Variables

		#region Handling Settings

		[FoldoutGroup("Handling Settings"), 
		 Tooltip("Max speed the vehicle can go")]
		[SerializeField] private float maxSpeed = 150f;

		[FoldoutGroup("Handling Settings"),
		 Tooltip("X = Time in seconds\n Y = Speed at that time. (in percentages from maxSpeed)")]
		[SerializeField] private AnimationCurve acceleration = AnimationCurve.Linear(0, 0, 3, 1),
												 deceleration = AnimationCurve.EaseInOut(0, 1, 2, 0);

		[FoldoutGroup("Handling Settings"), 
		 Tooltip("Percentage of velocity maintained when no input is given")]
		[SerializeField] private float slowingFactor = .98f;

		[FoldoutGroup("Handling Settings"), 
		 Tooltip("Percentage of velocity maintained when braking")]
		[SerializeField] private float brakingFactor = .95f;

		[Header("Physics Settings")] 
		
		[FoldoutGroup("Handling Settings")] 
		[SerializeField] private LayerMask groundLayer = 1 << 9, 
											wallLayer = 1 << 10;

		[FoldoutGroup("Handling Settings"), DisplayAsString, MinValue(3), MaxValue(21),
		 InlineButton("IncrementEven", ">"),
		 InlineButton("DecrementEven", "<")]
		[SerializeField] private int groundCheckResolution = 3;
		private void IncrementEven()
		{
			groundCheckResolution += 2;
		}
		private void DecrementEven()
		{
			groundCheckResolution -= 2;
		}

		[FoldoutGroup("Handling Settings"),
		 Tooltip("Distance the vehicle can be above the ground before it is 'falling'")]
		[SerializeField] private float maxGroundDist = 3f;

		[FoldoutGroup("Handling Settings"), 
		 Tooltip("Local gravity when the vehicle is on the ground")]
		[SerializeField] private float normalGravity = 15f;

		[FoldoutGroup("Handling Settings"),
		 Tooltip("Local gravity when the vehicle is falling")] 
		[SerializeField] private float fallGravity = 50f;

		#endregion

		#region Juice Settings

		[FoldoutGroup("Juice Settings")]
		[SerializeField] private float hoverHeight = 2f, angleOfRoll = 25f;

		[FoldoutGroup("Juice Settings"), SceneObjectsOnly]
		[SerializeField] private Camera vehicleCamera;

		[ToggleGroup("Juice Settings/enableScreenshake", "Enable Screenshake")]
		[SerializeField] private bool enableScreenshake;

		[ToggleGroup("Juice Settings/enableScreenshake"),
		 Tooltip("X = Percentage current speed compared to maximum speed.\n Y = Screenshake at that speed.")]
		[SerializeField] private AnimationCurve screenShakeCurve = new AnimationCurve(
			new Keyframe(0.0f, 0.0f, 0f, 0f),
						new Keyframe(0.6f, 0.0f, 0f, 5f),
						new Keyframe(0.6f, 0.3f, 0f, 0f),
						new Keyframe(1.0f, 1.0f, 0f, 0f));

		[ToggleGroup("Juice Settings/enableScreenshake"),
		 Tooltip("The direction the camera gets kicked in when you hit something.")]
		[SerializeField] private Vector3 hitKickDirection = Vector3.up;

		[ToggleGroup("Juice Settings/enableScreenshake"),
		 Tooltip("The amount of 'kickback' the camera gets when you hit something.")]
		[SerializeField] private float hitKickMultiplier = 3f;

		#endregion

		#region Object References

		[FoldoutGroup("Object References"), Space(5), SceneObjectsOnly, Required] 
		[SerializeField] private BoxCollider vehicleBody;

		[FoldoutGroup("Object References")]
		[SerializeField] private List<Axle> axles = new List<Axle>();

		#endregion

		[Header("Hover Settings")] public float hoverForce = 300f; //The force of the vehicle's hovering
		public PIDController hoverPID; //A PID controller to smooth the vehicle's hovering

		#endregion

		#region Non-Serialized Variables

		private float speed;
		private float rollAngle;
		private Rigidbody rigidBody;
		private VehicleInput input;
		private Vector3 centerOfMass;
		private float drag;
		private bool isOnGround;
		private float oldSpeedPercentage;

		private bool thrustGoingUp = false, thrustGoingDown = false;
		private float thrustLastFrame = 0f, timeSinceChangeInThrust = 0f;

		private bool isPlayer = true;
		private int playerIndex = 0;

		/// <summary>
		/// What percentage the current speed is of MaxSpeed (0f - 1f)
		/// </summary>
		private float SpeedPercentage
		{
			get
			{
				if (rigidBody != null)
				{
					return (rigidBody.velocity.magnitude / maxSpeed);
				}

				Debug.LogError("No Rigidbody attached to gameObject");
				return 0;
			}
		}

		private float CurrentShakeMultiplier
		{
			get { return screenShakeCurve.Evaluate(SpeedPercentage); }
		}

		[Serializable]
		private class Axle
		{
			[FoldoutGroup("$axleName")] public string axleName;
			[FoldoutGroup("$axleName")] public bool useForSteering = false;
			[FoldoutGroup("$axleName")] public Wheel leftWheel, rightWheel;
		}

		[Serializable]
		private class Wheel
		{
			public GameObject wheelTransform;
			public float wheelDiameter = 1f;
			public bool drift = false;
		}

		#endregion

		#endregion

		#region Methods

		private void Start()
		{
			rigidBody = GetComponent<Rigidbody>();
			input = GetComponent<PlayerInput>();

			//drag = driveForce / maxSpeed; //TODO: EDIT
		}

		private void FixedUpdate()
		{
			oldSpeedPercentage = SpeedPercentage;

			speed = Vector3.Dot(rigidBody.velocity, transform.forward);

			timeSinceChangeInThrust += 1f * Time.fixedDeltaTime;

			if ((input.Thrust > thrustLastFrame) && (!thrustGoingUp || thrustGoingDown))
			{
				thrustGoingDown = false;
				thrustGoingUp = true;

				timeSinceChangeInThrust = 0f;
			}

			if ((input.Thrust < thrustLastFrame) && (!thrustGoingDown || thrustGoingUp))
			{
				thrustGoingUp = false;
				thrustGoingDown = true;

				timeSinceChangeInThrust = 0f;
			}

			thrustLastFrame = input.Thrust;

			CalculateHover();
			CalculatePropulsion();

			centerOfMass = rigidBody.centerOfMass;

			#if UNITY_EDITOR
			DebugExtension.DebugWireSphere(position: this.transform.position + centerOfMass, radius: 0.1f,
				color: Color.cyan, duration: 0.1f);
			#endif
		}

		private void CalculateHover()
		{
			List<RaycastHit> positiveHits = new List<RaycastHit>();

			List<float> allHeightsFromGround = new List<float>();

			List<Vector3> allGroundNormals = new List<Vector3>();

			for (int groundCheckX = 0; groundCheckX < groundCheckResolution; groundCheckX++)
			{
				for (int groundCheckZ = 0; groundCheckZ < groundCheckResolution; groundCheckZ++)
				{
					Vector3 vehicleBodySize = vehicleBody.size;

					Vector3 startPos = new Vector3(
						vehicleBodySize.x / 2f,
						0,
						vehicleBodySize.z / 2f);

					Vector3 sizeMultiplier = new Vector3(
						vehicleBodySize.x / (groundCheckResolution - 1f),
						0,
						vehicleBodySize.z / (groundCheckResolution - 1f));

					Vector3 rayCastPos = new Vector3(
						groundCheckX * sizeMultiplier.x,
						vehicleBodySize.y / 2,
						groundCheckZ * sizeMultiplier.z) - startPos;

					rayCastPos = vehicleBody.transform.TransformPoint(rayCastPos);

					RaycastHit hitInfo = new RaycastHit();

					Ray ray = new Ray(rayCastPos, -transform.up);
					if (Physics.Raycast(ray, out hitInfo, maxGroundDist, groundLayer))
					{
						positiveHits.Add(hitInfo);
						allHeightsFromGround.Add(hitInfo.distance);
						allGroundNormals.Add(hitInfo.normal.normalized);
					}

					Debug.DrawRay(ray.origin, ray.direction * maxGroundDist, Color.magenta);
				}
			}

			isOnGround = (positiveHits.Count >= Mathf.Round(groundCheckResolution * groundCheckResolution / 2f));

			Vector3 averageGroundNormal;

			if (isOnGround)
			{
				float averageHeightFromGround = allHeightsFromGround.Average(); //Get average height from ground

				averageGroundNormal = new Vector3(
					allGroundNormals.Average(x => x.x),
					allGroundNormals.Average(x => x.y),
					allGroundNormals.Average(x => x.z)); //Get average ground normal.

				float forcePercent = hoverPID.Seek(hoverHeight, averageHeightFromGround);

				Vector3 force = averageGroundNormal * hoverForce * forcePercent;

				Vector3 gravity = -averageGroundNormal * normalGravity * averageHeightFromGround;

				rigidBody.AddForce(force, ForceMode.Acceleration);
				rigidBody.AddForce(gravity, ForceMode.Acceleration);

				vehicleCamera.Shake((CurrentShakeMultiplier / 10) * Mathf.Abs(input.Steer));
			}
			else
			{
				averageGroundNormal = Vector3.up;

				Vector3 gravity = -averageGroundNormal * fallGravity;
				rigidBody.AddForce(gravity, ForceMode.Acceleration);
			}

			Vector3 projection = Vector3.ProjectOnPlane(transform.forward, averageGroundNormal);
			Quaternion rotationToGround = Quaternion.LookRotation(projection, averageGroundNormal);

			if (isOnGround)
			{
				rigidBody.MoveRotation(Quaternion.Lerp(rigidBody.rotation, rotationToGround,
					Time.fixedDeltaTime * 15f));
			}
			else
			{
				if (input.IsStunting)
				{
					//TODO: Stunt movement.	
				}
				else
				{
					rigidBody.MoveRotation(Quaternion.Lerp(rigidBody.rotation, rotationToGround,
						Time.fixedDeltaTime * 1f));
				}
			}

			rollAngle = (angleOfRoll * -input.Steer) * SpeedPercentage;

			Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0f, 0f, rollAngle);
			vehicleBody.transform.rotation =
				Quaternion.Lerp(vehicleBody.transform.rotation, bodyRotation, Time.fixedDeltaTime * 10f);
		}

		private void CalculatePropulsion()
		{
			float forwardSpeed = Vector3.Dot(rigidBody.velocity, transform.forward);

			float yawRotationTorque =
				((forwardSpeed < -0.1f) ? -input.Steer : input.Steer) - rigidBody.angularVelocity.y;

			rigidBody.AddRelativeTorque(0f, yawRotationTorque, 0f, ForceMode.VelocityChange);

			float sidewaysSpeed = Vector3.Dot(rigidBody.velocity, transform.right);

			Vector3 sideFriction = -transform.right * (sidewaysSpeed / (Time.fixedDeltaTime / 0.5f));

			rigidBody.AddForce(sideFriction, ForceMode.Acceleration);

			if (input.Thrust <= 0f)
			{
				rigidBody.velocity *= slowingFactor;
			}

			if (isOnGround)
			{
				if (input.IsDrifting)
				{
					rigidBody.velocity *= brakingFactor;
				}

				float currentAcceleration = 0;

				if (thrustGoingUp)
				{
					currentAcceleration = acceleration.Evaluate(timeSinceChangeInThrust);
				}
				else if (thrustGoingDown)
				{
					currentAcceleration = deceleration.Evaluate(timeSinceChangeInThrust);
				}

				float propulsion = (currentAcceleration * maxSpeed) * input.Thrust;

				//TODO: Edit
				rigidBody.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			bool isWallLayer = (wallLayer != (wallLayer | (1 << collision.gameObject.layer)));

			vehicleCamera.Kick(hitKickDirection, (hitKickMultiplier * (isWallLayer ? 10f : 1f) * oldSpeedPercentage));
		}

		private void OnCollisionStay(Collision collision)
		{
			if ((wallLayer != (wallLayer | (1 << collision.gameObject.layer)))) { return; }

			vehicleCamera.Shake((CurrentShakeMultiplier / 10) * oldSpeedPercentage*2);

			Vector3 upwardForceFromCollision = Vector3.Dot(collision.impulse, transform.up) * transform.up;
			rigidBody.AddForce(-upwardForceFromCollision, ForceMode.Impulse);
		}

		#endregion
	}
}