public interface IDamageable
{
    void Damage(float damageAmount);

    void Die();

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}
