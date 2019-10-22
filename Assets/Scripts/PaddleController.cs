 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Breakout
{
    public class PaddleController : MonoBehaviour
    {
        private PlayerControls controls;
        // This isn't even being used so refactor to use it or remove it
        private Vector2 lastMousePosition;
        [SerializeField]
        private float speed = 1f;
        private Camera gameCamera;
        private Vector3 startingPosition;
        [SerializeField]
        private bool isFrozen = false;
        private void Awake()
        {
            controls = new PlayerControls();
            controls.Gameplay.Slide.performed += ctx => lastMousePosition = ctx.ReadValue<Vector2>();
            controls.Gameplay.Slide.canceled += ctx => lastMousePosition = Vector2.zero;
        }

        private void Start()
        {
            gameCamera = Camera.main;
            startingPosition = transform.position;
            if (isFrozen)
            {
                transform.position = new Vector3(startingPosition.x, startingPosition.y);
                transform.localScale += new Vector3(17f, 0);
                return;
            }
        }

        private void Update()
        {
            if (isFrozen) {
                transform.position = new Vector3(startingPosition.x, startingPosition.y);
                return;
            }
            Vector3 newMousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(newMousePosition.x, startingPosition.y);
        }

        private void OnEnable()
        {
            controls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            controls.Gameplay.Disable();
        }
    }

}