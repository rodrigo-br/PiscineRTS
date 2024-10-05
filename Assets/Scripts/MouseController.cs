using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    public Vector2 TargetPosition {get; private set; }
    [SerializeField] private float _movementSpeed = 5f;
    private InputManager _input;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        TargetPosition = transform.position;
    }

    private void Update()
    {
        FrameInput = _input.GatherInput();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (FrameInput.MouseLeftClick)
        {
            TargetPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        }
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Time.deltaTime * _movementSpeed);
    }
}
