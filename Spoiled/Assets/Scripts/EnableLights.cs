using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLights : MonoBehaviour
{
    public List<Light> lights;
    public List<GameObject> lamps;
    public Material lampOnMaterial;
    public Material lampOffMaterial;
    void Start()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLights(float time)
    {
        if (lamps.Count > 0)
        {
            foreach(Light light in lights)
            {
                light.enabled = true;
                foreach(GameObject lamp in lamps)
                {
                    SetLampMaterial(lamp, lampOnMaterial);
                    StartCoroutine(EnableLight(lamp, light, time));
                }
            }
        }
    }

    public IEnumerator EnableLight(GameObject lamp,Light light, float time)
    {
        yield return new WaitForSeconds(time);
        light.enabled = false;
        SetLampMaterial(lamp, lampOffMaterial);
    }

    private void SetLampMaterial(GameObject lamp, Material material)
    {
        if (lamp != null)
        {
            MeshRenderer renderer = lamp.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                materials[1] = material;
            }
        }
    }
}
