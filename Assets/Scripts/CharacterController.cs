using System;
using UnityEngine;

public class CharacterController : MonoBehaviour, ISelectableUnit
{
    public Action<bool> OnSelected;
    public Vector2 TargetPosition { get; private set; }
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _attackSpeed = 1f;
    private Transform _damageableTarget;
    private float _currentAttackTime = 0f;

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
        if (_damageableTarget == null) { return; }
        _currentAttackTime += Time.deltaTime;
        if (_currentAttackTime < _attackSpeed) { return; }
        if (Vector2.Distance(transform.position, TargetPosition) < 0.1f)
        {
            _currentAttackTime = 0f;
            _damageableTarget.GetComponentInChildren<IDamageable>().TakeDamage(10);
        }
    }

    public void SetTargetPosition(Vector2 newTargetPosition)
    {
        TargetPosition = newTargetPosition;
    }

    public void SetDamageableTarget(Transform damageableTarget)
    {
        _damageableTarget = damageableTarget;
    }
}
