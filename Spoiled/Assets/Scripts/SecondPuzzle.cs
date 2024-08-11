using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SecondPuzzlePart
{
    public GameObject fruit;
    public GameObject plate;
    public GameObject parkourPart;
}

public class SecondPuzzle : MonoBehaviour
{
    [SerializeField] List<SecondPuzzlePart> listOfParts = new List<SecondPuzzlePart>();
    [SerializeField] GameObject puzzleSign;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        CheckForMatches();

        if (time > 1 && !puzzleSign.active)
        {
            time = 0;
            CheckForAnotherColission();
        }
    }

    void CheckForMatches()
    {
        foreach(SecondPuzzlePart part in listOfParts)
        {
            if (IsColliding(part.fruit, part.plate))
            {
                part.parkourPart.SetActive(true);
            }

            else if (part.parkourPart.active)
            {
                part.parkourPart.SetActive(false);
            }
        }
    }

    void CheckForAnotherColission()
    {
        foreach(SecondPuzzlePart part in listOfParts)
        {
            foreach(SecondPuzzlePart part2 in listOfParts)
            {
                if (IsColliding(part.fruit, part2.plate))
                {
                    puzzleSign.SetActive(true);
                }
            }
        }
    }

    bool IsColliding(GameObject obj1, GameObject obj2)
    {
        // Get the colliders of both objects
        Collider col1 = obj1.GetComponent<Collider>();
        Collider col2 = obj2.GetComponent<Collider>();

        // If either collider is missing, return false
        if (col1 == null || col2 == null) return false;

        // Use the Bounds of the colliders to check for intersection
        return col1.bounds.Intersects(col2.bounds);
    }
}
