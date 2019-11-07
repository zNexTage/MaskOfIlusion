﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    Image HealthBar;
    float MaxHealth = 100f;
    public static float health;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GetComponent<Image>();
        health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = health / MaxHealth;
    }
}
