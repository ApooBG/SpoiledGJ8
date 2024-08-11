using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    // Reference to the player or target transform
    public Transform playerTransform;
    public bool reversed; //leave false for items, true for this object

    // List of items that should look at the player
    public List<RectTransform> itemsToLookAtPlayer;

    void Update()
    {
        if (playerTransform != null && reversed == false)
        {
            foreach (var item in itemsToLookAtPlayer)
            {
                if (item != null)
                {
                    Vector3 direction = item.position - playerTransform.position;
                    direction.y = 0; // Keep the item upright
                    item.rotation = Quaternion.LookRotation(direction);
                }
            }
        }

        else if (reversed)
        {
            Vector3 direction = gameObject.transform.position - playerTransform.position;
            direction.y = 0; // Keep the item upright
            gameObject.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
