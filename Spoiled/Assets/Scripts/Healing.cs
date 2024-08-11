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
        healthCanvas.gameObject.SetActive(!rotting.enabled);

        if (distance < distanceNeeded)
        {
            if (healingStorage > 0)
            {
                UpdateUI();
                if (time > 1f)
                {
                    Heal();
                    time = 0;
                }
                rotting.enabled = false;
            }

            else
            {
                rotting.enabled = true;
            }
        }

        else
        {
            rotting.enabled = true;
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
        Debug.Log(healingStorage / maximumHealingStorage);
        Debug.Log("Healing " + healingStorage + " /" + maximumHealingStorage);

    }

    void Heal()
    {
        if (healingPerSecond <= healingStorage)
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
