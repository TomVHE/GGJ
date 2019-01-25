//This script handles reading inputs from the player and passing it on to the vehicle. We 
//separate the input code from the behaviour code so that we can easily swap controls 
//schemes or even implement and AI "controller". Works together with the VehicleMovement script

using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInput : VehicleInput
{
	[SerializeField] private string verticalAxisName = "Vertical", horizontalAxisName = "Horizontal";
	[SerializeField] private string driftKey = "Jump";
	//[SerializeField] private new bool stuntingSameAsDrifting = false;
	[HideIf("StuntingSameAsDrifting")] 
	[SerializeField] private string stuntKey = "X";
	
	private void Update()
	{
		/*
		//If a GameManager exists and the game is not active...
		if (GameManager.instance != null && !GameManager.instance.GameIsActive)
		{
			//...set all inputs to neutral values and exit this method
			thruster = rudder = 0f;
			isBraking = false;
			return;
		}
		*/

		Thrust = Input.GetAxis(verticalAxisName);
		Steer = Input.GetAxis(horizontalAxisName);
		IsDrifting = Input.GetButton(driftKey);
		
		if(StuntingSameAsDrifting){return;}

		IsStunting = Input.GetButton(stuntKey);
	}
}
