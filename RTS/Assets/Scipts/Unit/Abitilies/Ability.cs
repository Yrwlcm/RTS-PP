using System;
using JetBrains.Annotations;
using Scipts;
using UnityEngine;

[Serializable]
public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public KeyCode hotkey;
    public float cooldown;
    public float lastUsedTime = 0f;

    public Ability InstantiateAndInitialize()
    {
        var abilityInstance = Instantiate(this);
        abilityInstance.Initialize();
        return abilityInstance;
    }
    
    protected virtual void Initialize()
    {
        // Инициализация способности
        // Эта строчка необходима чтобы приватный кулдаун абилки сбрасывался при каждом старте
        lastUsedTime = -cooldown;
    }

    public void Use(GameObject user, [CanBeNull] GameObject target = null)
    {
        if (Time.time - lastUsedTime >= cooldown)
        {
            lastUsedTime = Time.time;
            ApplyEffect(user, target);
            Debug.Log(abilityName + " used!");
        }
        else
        {
            Debug.Log(abilityName + " is on cooldown!");
        }
    }

    protected virtual void ApplyEffect(GameObject user, [CanBeNull] GameObject target = null)
    {
        // Применение эффекта способности
    }
}