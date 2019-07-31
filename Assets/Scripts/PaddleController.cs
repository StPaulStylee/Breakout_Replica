 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    private PlayerControls controls;
    private float movement;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Slide.performed += ctx => movement = ctx.ReadValue<float>() / 2;
        //controls.Gameplay.Slide.performed += ctx => Debug.Log("Value: " + (ctx.ReadValue<float>() / 100));
        controls.Gameplay.Slide.canceled += ctx => movement = 0f;
    }

    private void Update()
    {
        Debug.Log(movement);
        float direction = movement * Time.deltaTime;
        Vector2 position = new Vector2(direction, 0);
        transform.Translate(position, Space.World);
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
