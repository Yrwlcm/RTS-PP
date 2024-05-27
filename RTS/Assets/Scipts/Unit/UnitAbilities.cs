using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitAbilities : MonoBehaviour
{
    public Ability[] Abilities;

    public void Update()
    {
        foreach (var ability in Abilities)
        {
            if (Input.GetKeyDown(ability.hotkey))
            {
                ability.Use();
            }
        }
    }
}