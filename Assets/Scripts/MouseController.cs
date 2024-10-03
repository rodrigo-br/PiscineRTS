using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static Action<bool> OnMovementChange;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Animator _animator;
    private InputManager _input;
    private FrameInput _frameInput;
    private Vector2 _targetPosition;
    private bool _isMoving;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _targetPosition = transform.position;
        _animator.Play("Knight_Idle_N_Animation");
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
        float distance = Vector2.Distance(transform.position, _targetPosition);
        
        if (distance <= 0.2f && _isMoving)
        {
            _isMoving = false;
        }
        else if (distance > 0.2f && !_isMoving)
        {
            _isMoving = true;
        }
        else if (!_isMoving)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }


        if (angle >= 337.5f || angle < 22.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_W_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_W_Animation");
            }
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_SW_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_SW_Animation");
            }
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_S_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_S_Animation");
            }
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_SE_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_SE_Animation");
            }
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_E_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_E_Animation");
            }
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_NE_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_NE_Animation");
            }
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_N_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_N_Animation");
            }
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            if (_isMoving)
            {
                _animator.Play("Knight_Run_NW_Animation");
            }
            else
            {
                _animator.Play("Knight_Idle_NW_Animation");
            }
        }
    }
}
