using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthUI 
{ 
    public GameObject emoji;
    public float healthStart;
}

public class PlayerUI : MonoBehaviour
{
    [SerializeField] List<HealthUI> healthUIList;
    [SerializeField] Rotting rotting;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject emoji;
    [SerializeField] GameObject surpriseEmoji;


    int rottenPercentage;
    // Start is called before the first frame update
    void Start()
    {
        rottenPercentage = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (rottenPercentage != rotting.rottenPercentage)
        {
            rottenPercentage = rotting.rottenPercentage;
            UpdateUI();
        }
    }


    void UpdateUI()
    {
        healthBar.fillAmount = (100-rottenPercentage) / 100f;
        Debug.Log((100f - rottenPercentage) / 100);
        
        foreach (HealthUI healthUI in healthUIList)
        {
            if (rottenPercentage == healthUI.healthStart)
            {
                GameObject newEmoji = Instantiate(healthUI.emoji);

                // Set the parent of the new object to be the same as gameObject2's parent
                newEmoji.transform.SetParent(emoji.transform.parent, false);

                // Match the position, rotation, and scale
                newEmoji.transform.position = emoji.transform.position;
                newEmoji.transform.rotation = emoji.transform.rotation;
                newEmoji.transform.localScale = emoji.transform.localScale;
                Destroy(emoji);
                this.emoji = newEmoji;
            }
        }
    }
    
}
