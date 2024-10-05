using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public static Action<bool> OnMovementChange;
    [SerializeField] private SpriteRenderer _selectionUI;
    private Animator _animator;
    private CharacterController _characterController;
    private bool _isMoving;

    private void Awake()
    {
        _characterController = GetComponentInParent<CharacterController>();
        _animator = GetComponent<Animator>();
        _animator.Play("Knight_Idle_N_Animation");
    }

    private void OnEnable()
    {
        _characterController.OnSelected += SetSelectionUI;
    }

    private void OnDisable()
    {
        _characterController.OnSelected -= SetSelectionUI;
    }

    private void Update()
    {
        HandleSpriteAnimation();
    }

    private void HandleSpriteAnimation()
    {
        Vector2 direction = ((Vector2)transform.position - _characterController.TargetPosition).normalized;
        float distance = Vector2.Distance(transform.position, _characterController.TargetPosition);

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

    public void TriggerOnMovementChange()
    {
        OnMovementChange?.Invoke(true);
    }

    private void SetSelectionUI(bool state)
    {
        _selectionUI.enabled = state;
    }

}
