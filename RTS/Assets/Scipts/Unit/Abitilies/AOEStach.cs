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
            Debug.Log("Найден объект: " + collider.gameObject.name);
            var unit = collider.gameObject.GetComponent<Unit>();
            var myUnit = user.GetComponent<Unit>();
            if (unit != null && unit.Team != myUnit.Team)
        }
        return true;
    }
}
