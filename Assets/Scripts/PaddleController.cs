 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 movement;
    [SerializeField]
    private float speed = 1f;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Slide.performed += ctx => movement = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Slide.performed += ctx => Debug.Log("Value: " + (ctx.ReadValue<float>() / 100));
        controls.Gameplay.Slide.canceled += ctx => movement = Vector2.zero;
    }

    private void Update()
    {
        Debug.Log("Movement: " + movement.x);
        //float direction = movement * Time.deltaTime;
        //Vector2 position = new Vector2(movement.x, 0) * speed * Time.deltaTime;
        if (movement.x < 0)
        {
            //Debug.Log("Left: " + Vector2.left * Time.deltaTime);
            //transform.Translate(Vector2.left * Time.deltaTime * (-movement.x / 2), Space.World);
            transform.Translate(new Vector2(movement.x / 100, 0));
            return;
        } else if (movement.x > 0) {
            //Debug.Log("Right: " + Vector2.right * Time.deltaTime);
            //transform.Translate(Vector2.right * Time.deltaTime * (movement.x / 2), Space.World);
            transform.Translate(new Vector2(movement.x / 100, 0));
        }
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
