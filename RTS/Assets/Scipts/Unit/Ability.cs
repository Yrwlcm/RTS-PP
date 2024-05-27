using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public string abilityName;
    public Sprite icon;
    public KeyCode hotkey;
    public float cooldown;
    public float lastUsedTime;

    public void Use()
    {
        if (Time.time - lastUsedTime >= cooldown)
        {
            lastUsedTime = Time.time;
            // Добавьте логику способности здесь
            Debug.Log(abilityName + " used!");
        }
        else
        {
            Debug.Log(abilityName + " is on cooldown!");
        }
    }
}
