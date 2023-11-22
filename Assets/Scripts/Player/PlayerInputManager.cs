using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        private PlayerControls playerControls;
        [Header("Movement")] [SerializeField] Vector2 moveMentInput;
        [SerializeField] public float horizontalInput, verticalInput;
        [SerializeField] public float moveAmount;

        [Header("CameraMovement")] [SerializeField]
        Vector2 cameraInput;

        [SerializeField] public float cameraHorizontalInput, cameraVerticalInput;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
            instance.enabled = false;
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => moveMentInput = i.ReadValue<Vector2>();
                playerControls.CameraMovement.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldGameSaveManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void Update()
        {
            HandlePlayerMovement();
            HandleCameraMovement();
        }

        void HandlePlayerMovement()
        {
            verticalInput = moveMentInput.y;
            horizontalInput = moveMentInput.x;
            moveAmount = Mathf.Clamp01(MathF.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            if (moveAmount > 0 && moveAmount <= 0.5f)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1f;
            }
        }

        void HandleCameraMovement()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }
    }
}