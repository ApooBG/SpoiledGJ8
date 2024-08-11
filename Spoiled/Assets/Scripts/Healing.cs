using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healing : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Rotting rotting;

    [SerializeField] float distanceNeeded = 0.03f;
    [SerializeField] int healingPerSecond = 5;
    [SerializeField] float maximumHealingStorage = 50;
    [SerializeField] float respawnTime = 10f;
    [SerializeField] Canvas healthCanvas;
    [SerializeField] Image healthBar;

    float healingStorage;
    float time;
    MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        healthCanvas.gameObject.SetActive(false);
        renderer = GetComponent<MeshRenderer>();
        healingStorage = maximumHealingStorage;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        time += Time.deltaTime;

        if (distance < distanceNeeded)
        {
            if (healingStorage > 0)
            {
                if (rotting.rottenPercentage > 0)
                {
                    UpdateUI();
                    if (time > 1f)
                    {
                        Heal();
                        time = 0;
                    }
                    healthCanvas.gameObject.SetActive(true);
                }
            }

            else
            {
                healthCanvas.gameObject.SetActive(false);
            }
        }

        else
        {
            healthCanvas.gameObject.SetActive(false);
        }

        if (healingStorage <= 0)
        {
            if (respawnTime > 0 && respawnTime < time)
            {
                time = 0;
                renderer.enabled = true;
                gameObject.GetComponent<Collider>().enabled = true;
                healingStorage = maximumHealingStorage;
            }
        }
    }

    void UpdateUI()
    {
        healthBar.fillAmount = healingStorage / maximumHealingStorage;
    }

    void Heal()
    {
        if (rotting.rottenPercentage < healingPerSecond && rotting.rottenPercentage <= healingStorage)
        {
            healingStorage -= rotting.rottenPercentage;
            rotting.rottenPercentage -= rotting.rottenPercentage;
        }

        else if (healingPerSecond <= healingStorage)
        {
            rotting.rottenPercentage -= healingPerSecond;
            healingStorage -= healingPerSecond;
        }

        else if (healingStorage > 0)
        {
            rotting.rottenPercentage -= Convert.ToInt16(healingStorage);
            healingStorage = 0;
        }

        if (healingStorage <= 0)
        {
            renderer.enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
