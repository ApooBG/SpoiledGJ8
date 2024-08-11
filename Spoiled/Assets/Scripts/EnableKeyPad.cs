using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class EnableKeyPad : MonoBehaviour
{
    public Canvas keyPad; 

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "TV")
        {
            keyPad.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "TV")
        {
            keyPad.enabled = false;
        }
    }
}
