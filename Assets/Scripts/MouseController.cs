using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    [SerializeField] private Transform _selectionAreaTransform;
    private List<CharacterController> _characterControllers;
    private InputManager _input;
    private Vector2 _leftMouseStartPosition;
    private float _lastClickTime;
    private float _offset = 0.1f;
    private int _enemyLayerMask;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _characterControllers = new List<CharacterController>();
        _selectionAreaTransform.gameObject.SetActive(false);
        _enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        FrameInput = _input.GatherInput();
        HandleLeftClickInput();
        HandleRightClickInput();
    }

    private void HandleLeftClickInput()
    {
        if (FrameInput.MouseLeftClick)
        {
            HandleControlInput();
            HandleLeftClickUnheld();
        }
        else if (FrameInput.MouseLeftReleased)
        {
            HandleLeftClickRelease();
        }
        else if (!FrameInput.MouseLeftClick && FrameInput.MouseLeftHeld)
        {
            HandleLeftClickHeld();
        }
    }

    private void HandleLeftClickRelease()
    {
        Vector2 leftMouseEndPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        if (Time.time - _lastClickTime < _offset ||
            Vector2.Distance(_leftMouseStartPosition, leftMouseEndPosition) < _offset) { return; }
        _selectionAreaTransform.gameObject.SetActive(false);
        HandleControlInput();
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(_leftMouseStartPosition, leftMouseEndPosition);
        foreach (Collider2D collider2D in collider2DArray)
        {
            CharacterController character = collider2D.GetComponent<CharacterController>();
            if (character != null)
            {
                _characterControllers.Add(character);
                character.OnSelected?.Invoke(true);
            }
        }
    }

    private void HandleLeftClickUnheld()
    {
        _lastClickTime = Time.time;
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        _leftMouseStartPosition = targetPosition;
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

    private void HandleLeftClickHeld()
    {
        if (Time.time - _lastClickTime < _offset) { return; }
        _selectionAreaTransform.gameObject.SetActive(true);
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);
        Vector2 lowerLeftPosition = new Vector2(
            Mathf.Min(_leftMouseStartPosition.x, currentMousePosition.x),
            Mathf.Min(_leftMouseStartPosition.y, currentMousePosition.y));
        Vector2 upperRightPosition = new Vector2(
            Mathf.Max(_leftMouseStartPosition.x, currentMousePosition.x),
            Mathf.Max(_leftMouseStartPosition.y, currentMousePosition.y));
        _selectionAreaTransform.position = lowerLeftPosition;
        _selectionAreaTransform.localScale = upperRightPosition - lowerLeftPosition;
    }

    private void HandleControlInput()
    {
        if (FrameInput.ControlHeld) { return; }
        foreach (var character in _characterControllers)
        {
            character.OnSelected?.Invoke(false);
        }
        _characterControllers.Clear();
    }

    private void HandleRightClickInput()
    {
        if (!FrameInput.MouseRightClick) { return; }
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(FrameInput.MousePosition);        

        HandleMovement(targetPosition);
    }

    private void HandleMovement(Vector2 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero, Mathf.Infinity, _enemyLayerMask);
        Transform damageableTarget = null;
        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponentInChildren<IDamageable>();
            if (damageable != null)
            {
                damageableTarget = hit.collider.transform;
                foreach (var character in _characterControllers)
                {
                    character.SetHitPosition(hit.collider.GetComponentInChildren<Health>().FindHitPosition(character.transform));
                }
            }
        }

        if (damageableTarget == null)
        {
            HandleNoDamageableTarget(targetPosition, damageableTarget);
        }
    }

    private void HandleNoDamageableTarget(Vector2 targetPosition, Transform damageableTarget)
    {
        List<Vector2> targetPositions = GetPositionListAround(targetPosition, new float[] { 0.5f, 1f, 1.5f }, new int[] { 5, 10, 20 });
        int targetPositionIndex = 0;

        foreach (var character in _characterControllers)
        {
            character.SetTargetPosition(targetPositions[targetPositionIndex]);
            targetPositionIndex = (targetPositionIndex + 1) % targetPositions.Count;
        }
    }

    private List<Vector2> GetPositionListAround(Vector2 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector2> positionList = new List<Vector2>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector2> GetPositionListAround(Vector2 startPosition, float distance, int positionCount)
    {
        List<Vector2> positionList = new List<Vector2>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector2 dir = ApllyRotationToVector(new Vector2(1, 0), angle);
            Vector2 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector2 ApllyRotationToVector(Vector2 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }
}
