using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    private InputManager _input;
    private FrameInput _frameInput;
    private Vector2 _targetPosition;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        _frameInput = _input.GatherInput();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_frameInput.MouseLeftClick)
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(_frameInput.MousePosition);
        }
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _movementSpeed);
    }
}
