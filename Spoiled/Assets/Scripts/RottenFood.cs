using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RottenFood : MonoBehaviour
{
    [SerializeField] float distanceNeeded = 0.15f;
    [SerializeField] float rottenTimePerUnit = 0.5f;

    [SerializeField] GameObject tomatoParent;
    [SerializeField] Image healthBar;
    [SerializeField] Rotting rotting;

    float time = 0;


    Renderer objectRenderer; // Reference to the Renderer component
    Color blinkColor = Color.red; // Color to blink
    float blinkInterval = 0.5f; // Time interval between blinks

    private Color originalColor; // Original color of the object
    private bool isBlinking = false; // Whether the object is currently blinking

    void Start()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    private IEnumerator Blink()
    {
        isBlinking = true;

        // Change color to blink color
        objectRenderer.material.color = blinkColor;

        // Wait for the blink interval
        yield return new WaitForSeconds(blinkInterval);

        // Revert back to original color
        objectRenderer.material.color = originalColor;

        // Wait for the blink interval again
        yield return new WaitForSeconds(blinkInterval);

        isBlinking = false;
    }

    private void LateUpdate()
    {
        float distance = Vector3.Distance(transform.position, tomatoParent.transform.position);

        time += Time.deltaTime;
        if (distance < distanceNeeded)
        {
            if (time > rottenTimePerUnit)
            {
                rotting.rottenPercentage++;
                time = 0;
                healthBar.color = Color.red;

                StartBlink();
            }
        }
    }

    void StartBlink()
    {
        if (objectRenderer != null)
        {
            // Start the blink coroutine if it's not already blinking
            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
        }
    }
}
