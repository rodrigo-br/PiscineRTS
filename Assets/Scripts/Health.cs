using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    public static Action<Health> OnHealthChange;
    public static Action<Health> OnDeath;
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [SerializeField] private Transform _healthPresenter;
    [SerializeReference] private Transform _hitPointsTransform;
    private SpriteRenderer _backgroundSpriteRenderer;
    public int CurrentHealth { get; private set; }
    private readonly int minMaxHealth = 10;
    private float _disableHealthUIAfterSeconds = 2f;
    private float _currentDisableHealthUITime = 0f;
    private List<HitPosition> _hitPositions = new List<HitPosition>();

    private void Awake()
    {
        _backgroundSpriteRenderer = GetComponent<SpriteRenderer>();
        ResetHealth();
    }

    private void OnEnable()
    {
        OnHealthChange += UpdateUI;
    }

    private void OnDisable()
    {
        OnHealthChange -= UpdateUI;
    }

    private void Start()
    {
        foreach (Transform hitPointTransform in _hitPointsTransform)
        {
            _hitPositions.Add(new HitPosition(hitPointTransform, transform));
        }
        OnHealthChange?.Invoke(this);
    }

    private void Update()
    {
        _currentDisableHealthUITime += Time.deltaTime;
        if (_currentDisableHealthUITime >= _disableHealthUIAfterSeconds)
        {
            SetHealthUI(false);
        }
    }

    private void UpdateUI(Health sender)
    {
        if (sender != this) { return; }

        _healthPresenter.localScale = new Vector3((float)CurrentHealth / MaxHealth, _healthPresenter.localScale.y, 1);
        _currentDisableHealthUITime = 0f;
        if (CurrentHealth != MaxHealth)
        {
            _currentDisableHealthUITime -= 4;
        }
        SetHealthUI(true);
    }

    public void ResetHealth()
    {
        ChangeHealthByAmount(MaxHealth);
    }

    private void ChangeHealthByAmount(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        OnHealthChange?.Invoke(this);
    }

    private void ChangeMaxHealthByAmount(int amount)
    {
        MaxHealth = Mathf.Clamp(MaxHealth + amount, minMaxHealth, int.MaxValue);
    }

    public void IncreaseMaxHealth(int amount)
    {
        ChangeMaxHealthByAmount(amount);
        ChangeHealthByAmount(amount);
    }

    public void DecreaseMaxHealth(int amount)
    {
        ChangeMaxHealthByAmount(-amount);
        if (this.CurrentHealth > minMaxHealth)
        {
            ChangeHealthByAmount(-amount);
        }
        else
        {
            OnHealthChange?.Invoke(this);
        }
    }

    public void TakeDamage(int amount)
    {
        ChangeHealthByAmount(-amount);
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject.transform.parent.parent.gameObject);
        }
    }

    public void Heal(int amount)
    {
        ChangeHealthByAmount(amount);
    }

    private void SetHealthUI(bool value)
    {
        _backgroundSpriteRenderer.enabled = value;
        _healthPresenter.gameObject.SetActive(value);
    }

    public HitPosition FindHitPosition(Transform attacker)
    {
        foreach (HitPosition hitPosition in _hitPositions)
        {
            if (!hitPosition.IsOccupied)
            {
                hitPosition.Occupy(attacker);
                return hitPosition;
            }
        }
        return null;
    }

    [Button]
    public void DebugTakeDamage()
    {
        TakeDamage(10);
    }

    [Button]
    public void DebugHeal()
    {
        Heal(10);
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}

public interface IHealable
{
    void Heal(int amount);
}