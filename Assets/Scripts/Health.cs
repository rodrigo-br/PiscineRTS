using NaughtyAttributes;
using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    public static Action<Health> OnHealthChange;
    public static Action<Health> OnDeath;
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [SerializeField] private Transform HealthPresenter;
    public int CurrentHealth { get; private set; }
    private readonly int minMaxHealth = 10;

    private void Awake()
    {
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
        OnHealthChange?.Invoke(this);
    }

    private void UpdateUI(Health sender)
    {
        HealthPresenter.localScale = new Vector3((float)CurrentHealth / MaxHealth, HealthPresenter.localScale.y, 1);
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