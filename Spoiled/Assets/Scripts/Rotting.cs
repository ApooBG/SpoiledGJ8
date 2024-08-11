using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotting : MonoBehaviour
{
    public Material targetMaterial;  // The material that will change over time
    public Material targetMaterial2;  // The material that will change over time

    public int rottenPercentage = 0; // The rotten percentage (0 to 100)
    public float rottenTimePerUnit = 0.5f;
    public float rottenTimePerUnitRunning = 0.4f;

    public PlayerMovement movement;

    private Color startMainColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);  // Starting color (255,0,0)
    private Color endMainColor = new Color(72f / 255f, 0f / 255f, 0f / 255f);    // End color (150,0,0)

    private Color startSecondColor = new Color(40f / 255f, 255f / 255f, 0f / 255f);  // Starting color (255,0,0)
    private Color endSecondColor = new Color(5f / 255f, 34f / 255f, 0f / 255f);    // End color (150,0,0)

    private float startMetallic = 0f;  // Starting metallic value
    private float endMetallic = 0.7f;  // End metallic value
    float time;

    private void Start()
    {
        time = 0;
        targetMaterial.color = startMainColor;
        targetMaterial2.color = startSecondColor;
    }

    void Update()
    {
        // Ensure rottenPercentage is within 0-100 range
        rottenPercentage = Mathf.Clamp(rottenPercentage, 0, 100);

        // Interpolate color and metallic value based on rottenPercentage
        Color currentColor = Color.Lerp(startMainColor, endMainColor, rottenPercentage / 100f);
        Color currentSecondaryColor = Color.Lerp(startSecondColor, endSecondColor, rottenPercentage / 100f);
        float currentMetallic = Mathf.Lerp(startMetallic, endMetallic, rottenPercentage / 100f);

        // Apply the color and metallic value to the material
        targetMaterial.color = currentColor;
        targetMaterial.SetFloat("_Metallic", currentMetallic);

        targetMaterial2.color = currentSecondaryColor;
        targetMaterial2.SetFloat("_Metallic", currentMetallic);

        time += Time.deltaTime;

        if (time > rottenTimePerUnit && !movement.isRunning)
        {
            time = 0;
            rottenPercentage++;
        }

        else if (time > rottenTimePerUnitRunning && movement.isRunning)
        {
            time = 0;
            rottenPercentage++;
        }
    }
}
