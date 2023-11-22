using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;
        public float HorizontalMovement;
        public float VerticalMovement;
        public float movement;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] private float walkingSpeed = 3;
        [SerializeField] private float RuningSpeed = 6;
        [SerializeField] private float rotationSpeed = 3;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void GetPlayerInputHorizontalAndVerticalValue()
        {
            HorizontalMovement = PlayerInputManager.instance.horizontalInput;
            VerticalMovement = PlayerInputManager.instance.verticalInput;
        }

        public void HandleAllMovement()
        {
            //1.GroundedMovement
            HandleGroundedMovement();
            //2.Rotation
            HandleRotation();
        }

        public void HandleGroundedMovement()
        {
            GetPlayerInputHorizontalAndVerticalValue();
            moveDirection = PlayerCamera.instance.transform.forward * VerticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * HorizontalMovement;
            moveDirection.y = 0;
            moveDirection.Normalize();
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                //run
                player.characterController.Move(moveDirection * RuningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                //walk
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        public void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * VerticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * HorizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero) {
                targetRotationDirection = transform.forward;
            }
            Quaternion turnRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, turnRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = newRotation;
        }
    }
}