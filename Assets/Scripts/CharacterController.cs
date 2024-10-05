using System;
using UnityEngine;

public class CharacterController : MonoBehaviour, ISelectableUnit
{
    public Action<bool> OnSelected;
    public Vector2 TargetPosition { get; private set; }
    [SerializeField] private float _movementSpeed = 5f;

    private void Awake()
    {
        TargetPosition = transform.position;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Time.deltaTime * _movementSpeed);
    }

    public void SetTargetPosition(Vector2 newTargetPosition)
    {
        TargetPosition = newTargetPosition;
    }
}
