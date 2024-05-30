using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scipts;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Abilities/TargetDamageAbility")]
public class TargetDamageAbility : Ability
{
    [SerializeField] private float damageAmount;

    protected override bool ApplyEffect(GameObject user, [CanBeNull] GameObject target = null)
    {
        if (target == null)
            return false;
        var targetUnit = target.GetComponent<Unit>();
        var userUnit = user.GetComponent<Unit>();
        if (targetUnit == null || targetUnit.Team == userUnit.Team)
            return false;
        var targetHealth = target.GetComponent<Health>();
        if (targetHealth == null)
            return false;

        targetHealth.TakeDamage(damageAmount);
        Debug.Log($"{user.name} damaged for {damageAmount} HP");
        return true;
    }
}