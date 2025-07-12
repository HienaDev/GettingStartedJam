using System;
using TMPro;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class DialogSystem : MonoBehaviour
{

    [SerializeField] private DialogueScript initialDialogueScript;


    [Header("Dialog System UI")]
    [SerializeField] private TextMeshProUGUI dialogBoxText;
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private float optionRotationSpeed = 15f;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera dialogueCamera;

    [Header("Items")]
    [SerializeField] private Item[] items;
    [SerializeField] private RectTransform centerOfOptions;
    [SerializeField] private float ellipseWidth = 400f;  // Horizontal radius (X)
    [SerializeField] private float ellipseHeight = 225f; // Vertical radius (Y) - 16:9 ratio


    [Header("Debug")]
    [SerializeField] private Transform placeToSpawnItems;

    private Dialogue[] currentDialogue;
    private int currentTextIndex = -1;
    private List<GameObject> spawnedOptions = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartNewDialogue(initialDialogueScript.dialogues);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartNewDialogue(Dialogue[] newDialogue)
    {
        currentDialogue = newDialogue;
        currentTextIndex = -1;
        dialogBox.SetActive(true);
        NextChat();
    }

    public void NextChat(string optionText = "")
    {
        if (optionText != "")
        {
            dialogBoxText.text = optionText;
            return;
        }

        currentTextIndex++;

        if (currentTextIndex >= currentDialogue.Length)
        {
            mainCamera.enabled = true;
            dialogueCamera.enabled = false;
            dialogBox.SetActive(false);
            nextButton.SetActive(false); // hide the next button
            return;
        }

        dialogBoxText.text = currentDialogue[currentTextIndex].text;

        if (currentDialogue[currentTextIndex].camera != null)
        {
            mainCamera.enabled = false;
            dialogueCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            dialogueCamera.enabled = false;
        }

        if (currentDialogue[currentTextIndex].hasItems)
        {
            GenerateRandomOptions();
            return;
        }

        if (currentDialogue[currentTextIndex].hasOptions)
        {
            ShowOptions(currentDialogue[currentTextIndex]);
            return;
        }




    }

    private void ShowOptions(Dialogue dialogue)
    {
        ClearSpawnedOptions(); // clear previously spawned buttons
        nextButton.SetActive(false); // hide the next button

        int count = dialogue.options.Length;

        for (int i = 0; i < count; i++)
        {
            GameObject option = Instantiate(optionPrefab, centerOfOptions);
            spawnedOptions.Add(option); // track it

            option.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.options[i].text;

            int index = i;
            if (dialogue.options[i].nextDialogue != null)
            {
                option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    nextButton.SetActive(true));
                option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    ClearSpawnedOptions());
                option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    StartNewDialogue(dialogue.options[index].nextDialogue.dialogues));

            }
            else
            {
                string answerText = dialogue.options[i].answerIFNoDialogue;
                option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    nextButton.SetActive(true));
                option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                     ClearSpawnedOptions());
                option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    NextChat(answerText));

            }

            // Before positioning
            float angle = i * Mathf.PI * 2f / count;
            Vector3 startPos = new Vector3(Mathf.Cos(angle) * ellipseWidth, Mathf.Sin(angle) * ellipseHeight, 0f);

            // Apply position
            RectTransform rect = option.GetComponent<RectTransform>();
            rect.localPosition = startPos;

            // Add and initialize rotation script
            var rotator = option.AddComponent<RotateAroundCenter>();
            rotator.Initialize(centerOfOptions, angle, ellipseWidth, ellipseHeight, optionRotationSpeed); // adjust speed as needed

        }
    }



    private void GenerateRandomOptions()
    {
        ClearSpawnedOptions(); // clear previously spawned buttons
        nextButton.SetActive(false); // hide the next button

        Item[] shuffledItems = (Item[])items.Clone();
        ShuffleArray(shuffledItems);

        Debug.Log(shuffledItems.Length);

        int count = Mathf.Min(3, shuffledItems.Length);

        for (int i = 0; i < count; i++)
        {
            Item item = shuffledItems[i];
            GameObject option = Instantiate(optionPrefab, centerOfOptions);
            spawnedOptions.Add(option);

            option.GetComponentInChildren<TextMeshProUGUI>().text = item.nameOfItem;
            option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                Instantiate(item.itemPrefab, placeToSpawnItems));
            option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                ClearSpawnedOptions());
            option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                NextChat(item.answersToItem[UnityEngine.Random.Range(0, item.answersToItem.Length)]));
            option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                nextButton.SetActive(true));

            // Before positioning
            float angle = i * Mathf.PI * 2f / count;
            Vector3 startPos = new Vector3(Mathf.Cos(angle) * ellipseWidth, Mathf.Sin(angle) * ellipseHeight, 0f);

            // Apply position
            RectTransform rect = option.GetComponent<RectTransform>();
            rect.localPosition = startPos;

            // Add and initialize rotation script
            var rotator = option.AddComponent<RotateAroundCenter>();
            rotator.Initialize(centerOfOptions, angle, ellipseWidth, ellipseHeight, optionRotationSpeed); // adjust speed as needed

        }
    }

    private void ClearSpawnedOptions()
    {
        Debug.Log("Cleared");
        foreach (GameObject option in spawnedOptions)
        {
            if (option != null)
                Destroy(option);
        }
        spawnedOptions.Clear();
    }


    private void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }


}
