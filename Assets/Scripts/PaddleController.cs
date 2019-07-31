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
        //controls.Gameplay.Slide.performed += ctx => Debug.Log("Value: " + (ctx.ReadValue<float>() / 100));
        controls.Gameplay.Slide.canceled += ctx => movement = Vector2.zero;
    }

    private void Update()
    {
        Debug.Log(movement);
        //float direction = movement * Time.deltaTime;
        Vector2 position = new Vector2(movement.x, 0) * Time.deltaTime;
        transform.Translate(position);
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
