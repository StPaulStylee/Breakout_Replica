 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 lastMousePosition;
    [SerializeField]
    private float speed = 1f;
    private Camera gameCamera;
    private Vector3 startingPosition;
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
    }

    private void Update()
    {
        Vector3 newMousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(newMousePosition);
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
