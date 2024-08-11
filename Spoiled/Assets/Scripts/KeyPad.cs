using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyPad : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ans;

    public Canvas keyPad;

    public GameObject ramp;

    public string answer = "4439";

    public void Start()
    {
        ramp.SetActive(false);
        keyPad.enabled = false;
    }
    public void Number(int number)
    {
        ans.text += number.ToString();
        if (ans.text.Length >= 4)
        {
            Execute();
        }
    }

    public void Execute()
    {
        if (ans.text == answer)
        {
            ramp.SetActive(true);
            ans.text = "Correct";
        }
        else 
        {
            ans.text = "XXXX";
            StartCoroutine(ShowIncorrectAndClear());
        }
    }
    private IEnumerator ShowIncorrectAndClear()
    {
        yield return new WaitForSeconds(1.3f);
        ans.text = "";
    }


}
