﻿using UnityEngine;
using UnityEngine.UI;

public class AntHomeCollider : MonoBehaviour
{
    private float health = 100f;

    public Image healthBar;

    private const float DAMAGE_TIME_INTERVAL = 1f;
    private const float COLOR_INTERPOLATION_TIME = 0.5f;

    private float damageTimer = 0f;
    private float interpolationTimer = COLOR_INTERPOLATION_TIME;

    private bool isTakingDamage = false;
    private static bool canTakeDamage;

    private void Awake()
    {
        CanTakeDamage(true);
    }

    private void Update()
    {
        if (isTakingDamage)
        {
            DisplayWarningSignal();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "RedAnt" && tag == "RedHome")
        {
            if (canTakeDamage)
                TakeDamage();
        }
        else if (collision.gameObject.tag == "BlueAnt" && tag == "BlueHome")
        {
            if (canTakeDamage)
                TakeDamage();
        }
    }

    private void TakeDamage()
    {
        isTakingDamage = true;

        damageTimer += Time.deltaTime;
        if (damageTimer > DAMAGE_TIME_INTERVAL)
        {
            if (health < 0.1f)
            {
                Debug.Log(tag + " is dead!");

                FindObjectOfType<CustomNetworkManagerUI>().ShowGameOverPanel(tag);
                CanTakeDamage(false);

                gameObject.SetActive(false);
                return;
            }
            
            // Reduce health
            health -= 10f;
            healthBar.fillAmount = health / 100f;

            // Reset damage timer
            damageTimer = 0f;
        }
    }

    private void DisplayWarningSignal()
    {
        if (interpolationTimer > 0f)
        {
            // Interpolate the health bar color from white to green
            healthBar.color = Color.Lerp(Color.green, Color.white, interpolationTimer / COLOR_INTERPOLATION_TIME);
            interpolationTimer -= Time.deltaTime;
        }
        else
        {
            isTakingDamage = false;
            interpolationTimer = COLOR_INTERPOLATION_TIME;
        }
    }

    static private void CanTakeDamage(bool flag)
    {
        canTakeDamage = flag;
    }
}