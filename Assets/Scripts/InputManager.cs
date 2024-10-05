using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private InputAction _mousePosition, _mouseLeftClick, _mouseRightClick;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _mousePosition = _inputActions.Gameplay.MousePosition;
        _mouseLeftClick = _inputActions.Gameplay.MouseLeftClick;
        _mouseRightClick = _inputActions.Gameplay.MouseRightClick;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    public FrameInput GatherInput()
    {
        return new FrameInput
        {
            MousePosition = _mousePosition.ReadValue<Vector2>(),
            MouseLeftClick = _mouseLeftClick.WasPressedThisFrame(),
            MouseLeftHeld = _mouseLeftClick.IsPressed(),
            MouseRightClick = _mouseRightClick.WasPressedThisFrame(),
        };
    }
}

public struct FrameInput
{
    public Vector2 MousePosition;
    public bool MouseLeftClick;
    public bool MouseLeftHeld;
    public bool MouseRightClick;
}

