using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public VisibilityChecker visibilityChecker;
    public bool isMoving;
    public bool isRotten;

    public string familyMember;
    public bool openDoor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (openDoor)
        {
            CheckFridgeDoorOpened(familyMember);
            //openDoor = false;
        }
    }


    void CheckFridgeDoorOpened(string person)
    {
        bool isVisible = visibilityChecker.IsTomatoVisible();
        Debug.Log("IsVisible " + isVisible);

        if (person == "Father" && isVisible)
        {
            if (isMoving)
            {
                HandleLoseCondition();
            }

            //safe
        }

        else if (person == "Child")
        {
            if (!isRotten && isVisible)
            {
                // Need to hide, otherwise lose.
                HandleLoseCondition();
            }

            if (isVisible && isMoving)
            {
                HandleLoseCondition();
            }

            //safe
        }
        else if (person == "Mother")
        {
            if (isVisible)
            {
                HandleLoseCondition();
            }

            //safe
        }
    }

    void HandleLoseCondition()
    {
        Debug.Log("Lost");
    }
}
