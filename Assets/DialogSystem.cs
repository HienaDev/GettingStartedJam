using System;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class DialogSystem : MonoBehaviour
{
    [Serializable]
    public struct Dialogue
    {
        public string name;
        public string text;
        public GameObject camera;

        public bool hasItems;
        [HideIf("hasItems")]
        public bool hasOptions;

        [ShowIf("hasOptions")]
        public Option[] options;
    }

    [Serializable]
    public struct Option
    {
        public string text;
        public Dialogue[] nextDialogue;

        public bool giveItems;
    }

    [SerializeField] private Dialogue[] initialDialogue;

    [Header("Dialog System UI")]
    [SerializeField] private TextMeshProUGUI dialogBoxText;
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject optionPrefab;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera dialogueCamera;

    [Header("Items")]
    [SerializeField] private Item[] items;

    [Header("Debug")]
    [SerializeField] private Transform placeToSpawnItems;

    private Dialogue[] currentDialogue;
    private int currentTextIndex = -1;   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartNewDialogue(initialDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartNewDialogue(Dialogue[] newDialogue)
    {
        currentDialogue = newDialogue;
        dialogBoxText.text = currentDialogue[currentTextIndex].text;
        currentTextIndex = -1;
        dialogBox.SetActive(true);
        NextChat();
    }

    public void NextChat()
    {
        currentTextIndex++;

        if(currentDialogue[currentTextIndex].camera != null)
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

        if(currentDialogue[currentTextIndex].hasOptions)
        {
            ShowOptions(currentDialogue[currentTextIndex]);
            return;
        }

        if (currentTextIndex >= currentDialogue.Length)
        {
            mainCamera.enabled = true;
            dialogueCamera.enabled = false;
            dialogBox.SetActive(false);
        }

        dialogBoxText.text = currentDialogue[currentTextIndex].text;
    }

    private void ShowOptions(Dialogue dialogue)
    {
        for (int i = 0; i < dialogue.options.Length; i++)
        {
            GameObject option = Instantiate(optionPrefab, dialogBox.transform);
            option.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.options[i].text;
            int index = i; // Capture the current index for the listener
            option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartNewDialogue(dialogue.options[i].nextDialogue));
        }
    }

    private void GenerateRandomOptions()
    {
        // Clone and shuffle the item list
        Item[] shuffledItems = (Item[])items.Clone();
        ShuffleArray(shuffledItems);

        // Spawn up to 3 unique options
        for (int i = 0; i < Mathf.Min(3, shuffledItems.Length); i++)
        {
            Item item = shuffledItems[i];
            GameObject option = Instantiate(optionPrefab, dialogBox.transform);
            option.GetComponentInChildren<TextMeshProUGUI>().text = item.nameOfItem;
            option.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Instantiate(item, placeToSpawnItems));
        }
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
