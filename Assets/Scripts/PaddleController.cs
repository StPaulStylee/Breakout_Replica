 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 movement;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Slide.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Gameplay.Slide.canceled += ctx => movement = Vector2.zero;
    }

    private void Update()
    {
        float direction = movement.x * Time.deltaTime;
        Vector2 move = new Vector2(direction, 0);
        transform.Translate(move, Space.World);
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
