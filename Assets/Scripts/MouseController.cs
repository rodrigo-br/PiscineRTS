using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private EightDirectionSprites _characterRunSO;
    [SerializeField] private float _spriteChangeSpeed = 0.1f;
    private float _currentSpriteTime;
    private List<Sprite> _currentRunSprites;
    private int _currentRunSpriteIndex;
    private InputManager _input;
    private FrameInput _frameInput;
    private Vector2 _targetPosition;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentRunSprites = _characterRunSO.SpritesN;
        _targetPosition = transform.position;
    }

    private void Update()
    {
        _frameInput = _input.GatherInput();
        HandleMovement();
        HandleSpriteAnimation();
    }

    private void HandleMovement()
    {
        if (_frameInput.MouseLeftClick)
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(_frameInput.MousePosition);
        }
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _movementSpeed);
    }

    private void HandleSpriteAnimation()
    {
        Vector2 direction = ((Vector2)transform.position - _targetPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        if (angle >= 337.5f || angle < 22.5f)
            _currentRunSprites = _characterRunSO.SpritesW;
        else if (angle >= 22.5f && angle < 67.5f)
            _currentRunSprites = _characterRunSO.SpritesSW;
        else if (angle >= 67.5f && angle < 112.5f)
            _currentRunSprites = _characterRunSO.SpritesS;
        else if (angle >= 112.5f && angle < 157.5f)
            _currentRunSprites = _characterRunSO.SpritesSE;
        else if (angle >= 157.5f && angle < 202.5f)
            _currentRunSprites = _characterRunSO.SpritesE;
        else if (angle >= 202.5f && angle < 247.5f)
            _currentRunSprites = _characterRunSO.SpritesNE;
        else if (angle >= 247.5f && angle < 292.5f)
            _currentRunSprites = _characterRunSO.SpritesN;
        else if (angle >= 292.5f && angle < 337.5f)
            _currentRunSprites = _characterRunSO.SpritesNW;

        _currentSpriteTime += Time.deltaTime;
        if (_currentSpriteTime >= _spriteChangeSpeed)
        {
            _currentSpriteTime = 0;
            _currentRunSpriteIndex++;
            if (_currentRunSpriteIndex >= _currentRunSprites.Count)
            {
                _currentRunSpriteIndex = 0;
            }
        }
        _spriteRenderer.sprite = _currentRunSprites[_currentRunSpriteIndex];
    }
}
