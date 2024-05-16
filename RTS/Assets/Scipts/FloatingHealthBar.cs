using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 offset;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
    }

    public void Start()
    {
        mainCamera = Camera.main;
    }

    public void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}
