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
    [SerializeField] private Image healthBarImage;

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

    public void SetColor(Color color)
    {
        healthBarImage.color = color;
    }
}
