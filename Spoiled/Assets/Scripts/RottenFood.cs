using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenFood : MonoBehaviour
{
    [SerializeField] float distanceNeeded = 0.15f;
    [SerializeField] float rottenTimePerUnit = 0.5f;

    [SerializeField] GameObject tomatoParent;
    [SerializeField] Rotting rotting;

    float time = 0;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, tomatoParent.transform.position);

        if (distance < distanceNeeded)
        {
            if (time > rottenTimePerUnit)
            {
                rotting.rottenPercentage++;
                time = 0;
            }
        }
    }
}
