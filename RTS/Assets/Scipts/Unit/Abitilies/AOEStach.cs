using JetBrains.Annotations;
using Scipts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Abilities/AOEAbility")]
public class AOEStach : Ability
{
    private float radius = 10f;
    protected override bool ApplyEffect(GameObject user, [CanBeNull] GameObject target = null)
    {
        var colliders = Physics.OverlapSphere(user.transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Debug.Log("������ ������: " + collider.gameObject.name);
            var unitHealth = collider.gameObject.GetComponent<Health>();
            var unit = collider.gameObject.GetComponent<Unit>();
            var myUnit = user.GetComponent<Unit>();
            if (unitHealth != null && unit != null && unit.Team != myUnit.Team)
            {
                unitHealth.TakeDamage(20);
            }
        }
        return true;
    }
}
