using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    public Transform tomato;                   // The tomato GameObject
    public List<Transform> fridgeDoorPositions; // A list of positions representing where the person might look from
    public LayerMask obstructionMask;          // A layer mask for objects that can obstruct visibility (e.g., shelves, bottles, etc.)



    public bool IsTomatoVisible()
    {
        // Check visibility from each position in the fridgeDoorPositions list
        foreach (Transform fridgeDoorPosition in fridgeDoorPositions)
        {
            // Calculate the direction from the observer to the tomato
            Vector3 directionToTomato = tomato.position - fridgeDoorPosition.position;
            //Debug.DrawRay(fridgeDoorPosition.position, directionToTomato, Color.red); // Visualize the ray
            // Perform the raycast
            RaycastHit hit;
            if (Physics.Raycast(fridgeDoorPosition.position, directionToTomato, out hit, Mathf.Infinity, obstructionMask))
            {
                // Check if the ray hit the tomato
                if (hit.transform == tomato)
                {
                    // Tomato is visible from this point
                    return true;
                }
            }
        }

        // If the tomato is not visible from any of the positions, consider it hidden
        return false;
    }
}
