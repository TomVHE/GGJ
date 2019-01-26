using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using KinematicCharacterController.Walkthrough.NavMeshExample;

namespace Core.Movement
{
    public class AI : MonoBehaviour
    {
        //public MyCharacterController character;
        public UberCharacterController character;
        [NonSerialized] public Vector3 destination;

        private NavMeshPath _path;
        private Vector3[] _pathCorners = new Vector3[16];
        private Vector3 _lastValidDestination;

        private void Start()
        {
            _path = new NavMeshPath();
        }

        private void Update()
        {
            HandleCharacterNavigation();
        }
        
        private void HandleCharacterNavigation()
        {
            AICharacterInputs aiCharacterInputs = new AICharacterInputs();
            
            if ((character.transform.position - destination).magnitude <= 0.2f)
            {

                aiCharacterInputs.MoveVector = Vector3.zero;

                // Apply inputs to character
                character.SetInputs(ref aiCharacterInputs);
     
                
                return;
            }            
            
            if(NavMesh.CalculatePath(character.transform.position, destination, NavMesh.AllAreas, _path))
            {
                _lastValidDestination = destination;
            }
            else
            {
                NavMesh.CalculatePath(character.transform.position, _lastValidDestination, NavMesh.AllAreas, _path);
            }

            int cornersCount = _path.GetCornersNonAlloc(_pathCorners); //Calculate path corners
            if (cornersCount >= 1)
            {
                // Build the CharacterInputs struct
                aiCharacterInputs.MoveVector = (_pathCorners[1] - character.transform.position).normalized;

                // Apply inputs to character
                character.SetInputs(ref aiCharacterInputs);
            }
            else
            {
                // Build the CharacterInputs struct
                aiCharacterInputs.MoveVector = Vector3.zero;

                // Apply inputs to character
                character.SetInputs(ref aiCharacterInputs);
            }
        }
    }
}