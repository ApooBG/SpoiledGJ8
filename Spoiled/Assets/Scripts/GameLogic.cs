using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public VisibilityChecker visibilityChecker;
    public PlayerMovement playerMovement;
    public Rotting rotting;
    public int minimumPercentageForTotteness = 35;
    public bool isMoving;
    public bool isRotten;

    public string familyMember;
    public bool openDoor = false;

    public float timeAlive = 0;
    public float timeToOpenDoor;
    bool isAlive = true;

    private string[] familyMembers = { "Child", "Mother", "Father" };

    private void Start()
    {
        timeToOpenDoor = Random.Range(120f, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            timeAlive += Time.deltaTime;
        }

        if (Mathf.Abs(timeToOpenDoor - timeAlive) <= 0.5f)
        {
            // Call the function or perform the action when the time is close enough
            OpenFridgeDoor();
        }

        if (rotting.rottenPercentage > minimumPercentageForTotteness)
            isRotten = true;
        else
            isRotten = false;

        if (playerMovement.isMoving)
            isMoving = true;
        else
            isMoving = false;

        if (openDoor)
        {
            CheckFridgeDoorOpened(familyMember);
            //openDoor = false;
        }

        if (rotting.rottenPercentage > 99)
        {
            HandleLoseCondition();
        }
    }

    void OpenFridgeDoor()
    {
        RandomTimeToOpenFridgeDoor();
        string chosenMember = familyMembers[Random.Range(0, familyMembers.Length)];
        Debug.Log("Family Member: " + chosenMember);
        CheckFridgeDoorOpened(chosenMember);
    }

    void RandomTimeToOpenFridgeDoor()
    {
        timeToOpenDoor = timeAlive + Random.Range(120f, 180f);
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
        //isAlive = false;
        Debug.Log("Lost");
    }
}
