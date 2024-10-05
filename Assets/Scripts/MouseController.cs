using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private List<CharacterController> _characterControllers;
    public FrameInput FrameInput { get; private set; }
    private InputManager _input;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
    }

    private void Update()
    {
        FrameInput = _input.GatherInput();
        HandleLeftClickInput();
        HandleRightClickInput();
    }

    private void HandleLeftClickInput()
    {
        if (!FrameInput.MouseLeftClick) { return; }
        foreach (var character in _characterControllers)
        {
            character.OnSelected?.Invoke(false);
        }
        _characterControllers.Clear();
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero);
        if (hit.collider != null)
        {
            CharacterController character = hit.collider.GetComponent<CharacterController>();
            if (character != null)
            {
                _characterControllers.Add(character);
                character.OnSelected?.Invoke(true);
            }
        }
    }

    private void HandleRightClickInput()
    {
        if (!FrameInput.MouseRightClick) { return; }
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        foreach (var character in _characterControllers)
        {
            character.SetTargetPosition(targetPosition);
        }
    }
}
