using System;
using UnityEngine;

public class CharacterController : MonoBehaviour, ISelectableUnit
{
    public Action<bool> OnSelected;
    public Vector2 TargetPosition { get; private set; }
    public bool IsAttacking { get; private set; }
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _attackSpeed = 1f;
    private Transform _damageableTarget;
    private float _currentAttackTime = 0f;
    private HitPosition _currentHitPosition;

    private void Awake()
    {
        TargetPosition = transform.position;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Time.deltaTime * _movementSpeed);
    }

    private void HandleAttack()
    {
        if (_damageableTarget == null) { IsAttacking = false; return; }
        _currentAttackTime += Time.deltaTime;
        if (_currentAttackTime < _attackSpeed) { return; }
        if (Vector2.Distance(transform.position, TargetPosition) < 0.1f)
        {
            IsAttacking = true;
            _currentAttackTime = 0f;
            _damageableTarget.GetComponentInChildren<IDamageable>().TakeDamage(10);
        }
        else
        {
            IsAttacking = false;
        }
    }

    public void SetHitPosition(HitPosition newHitPosition)
    {
        _currentHitPosition = newHitPosition;
    }

    public void SetTargetPosition(Vector2 newTargetPosition)
    {
        TargetPosition = newTargetPosition;
    }
}
