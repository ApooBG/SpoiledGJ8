using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class FamilyMember
{
    public string name;
    public Sprite sprite;
    public AudioClip audio;
}

public class GameLogic : MonoBehaviour
{
    [SerializeField] List<FamilyMember> familyMembers;
    public VisibilityChecker visibilityChecker;
    public PlayerMovement playerMovement;
    public Rotting rotting;
    public int minimumPercentageForTotteness = 35;
    public bool isMoving;
    public bool isRotten;
    public GameObject footsteps;

    public FamilyMember familyMember;
    public bool openDoor = false;

    public float timeAlive = 0;
    public float timeToOpenDoor;
    public float timeDoorStaysOpen;
    public GameObject DeadUI;

    float timeDoorStayedOpen = 0;

    public TextMeshProUGUI timeAliveText;
    bool isAlive = true;


    private void Start()
    {
        Time.timeScale = 1f;
        timeToOpenDoor = UnityEngine.Random.Range(120f, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        if (rotting.rottenPercentage == 100)
            isAlive = false;

        if (isAlive)
        {
            timeAlive += Time.deltaTime;
            timeAliveText.color = Color.yellow;
            timeAliveText.text = FormatTime(timeAlive);
        }

        else
        {
            HandleLoseCondition();
        }

        if (openDoor && isAlive)
        {
            timeAliveText.color = Color.red;
            timeDoorStayedOpen += Time.deltaTime;
            if (timeDoorStaysOpen < timeDoorStayedOpen)
            {
                timeDoorStayedOpen = 0;
                openDoor = false;
                footsteps.SetActive(false);
                Debug.Log("close door");
            }
        }

        if (Mathf.Abs(timeToOpenDoor - timeAlive) <= 7f && !footsteps.active)
        {
            BeforeFridgeOpens();
        }

        if (Mathf.Abs(timeToOpenDoor - timeAlive) <= 0.1f)
        {
            // Call the function or perform the action when the time is close enough
            openDoor = true;
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
        }

        if (rotting.rottenPercentage > 99)
        {
            HandleLoseCondition();
        }
    }


    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OpenFridgeDoor()
    {
        RandomTimeToOpenFridgeDoor();
        CheckFridgeDoorOpened(familyMember);
    }

    void RandomTimeToOpenFridgeDoor()
    {
        timeToOpenDoor = timeAlive + UnityEngine.Random.Range(120f, 180f);
    }

    void BeforeFridgeOpens()
    {
        familyMember = familyMembers[UnityEngine.Random.Range(0, familyMembers.Count)];
        footsteps.SetActive(true);
        footsteps.GetComponent<Image>().sprite = familyMember.sprite;
    }

    void CheckFridgeDoorOpened(FamilyMember person)
    {
        bool isVisible = visibilityChecker.IsTomatoVisible();
        Debug.Log("IsVisible " + isVisible);

        if (person.name == "Father" && isVisible)
        {
            if (isMoving)
            {
                HandleLoseCondition();
            }

            //safe
        }

        else if (person.name == "Child")
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
        else if (person.name == "Mother")
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
        isAlive = false;
        DeadUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
