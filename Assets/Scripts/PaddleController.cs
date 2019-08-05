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
    private Camera camera;
    private Vector3 startingPosition;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Slide.performed += ctx => lastMousePosition = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Slide.performed += ctx => Debug.Log("Value: " + (ctx.ReadValue<float>() / 100));
        controls.Gameplay.Slide.canceled += ctx => lastMousePosition = Vector2.zero;
    }

    private void Start()
    {
        camera = Camera.main;
        startingPosition = transform.position;
    }

    private void Update()
    {
        Vector3 newMousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(newMousePosition);
        //float direction = movement * Time.deltaTime;
        //Vector2 position = new Vector2(movement.x, 0) * speed * Time.deltaTime;
        //if (lastMousePosition.x < 0 || lastMousePosition.x > 0)
        //{
        //Debug.Log("Left: " + Vector2.left * Time.deltaTime);
        //transform.Translate(Vector2.left * Time.deltaTime * (-movement.x / 2), Space.World);
        //transform.Translate(new Vector2((movement.x / 100) * speed, 0));
        transform.position = new Vector3(newMousePosition.x, startingPosition.y);
            //return;
        //}
        //else if (lastMousePosition.x > 0) {
            //Debug.Log("Right: " + Vector2.right * Time.deltaTime);
            //transform.Translate(Vector2.right * Time.deltaTime * (movement.x / 2), Space.World);
            //transform.Translate(new Vector2((movement.x / 100) * speed, 0));
            //transform.Translate(Vector2.right * lastMousePosition.x);
        //}
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
