using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private List<CharacterController> _characterController;
    public FrameInput FrameInput { get; private set; }
    private InputManager _input;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
    }

    private void Update()
    {
        FrameInput = _input.GatherInput();
        HandleClickInput();
    }

    private void HandleClickInput()
    {
        if (!FrameInput.MouseLeftClick) { return; }
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        foreach (var character in _characterController)
        {
            character.SetTargetPosition(targetPosition);
        }
    }
}
