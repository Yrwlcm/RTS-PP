using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/HealAbility")]
public class HealAbility : Ability
{
    [SerializeField] private float healAmount;

    protected override void ApplyEffect(GameObject user, [CanBeNull] GameObject target = null)
    {
        // Добавьте здесь логику баффа
        var unitHealth = user.GetComponent<Health>();
        if (unitHealth == null)
            return;
        unitHealth.Heal(healAmount);
        Debug.Log($"{user.name} healed {healAmount} HP");
    }
}