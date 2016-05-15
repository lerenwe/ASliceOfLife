using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueDisplayer : MonoBehaviour {

    public string textToDisplay = "lol";
    public GameObject wordPrefab;
    public GameObject mainCanvas;
    string displayedText = null;
    char[] currentCharsToDisplay;
    string[] currentWordsToDisplay;
    float nextCharacterTimer;
    public float displaySpeedRate = 1f;
    public float spaceBetweenLines = .2f;
    public float xMargin = 0;
    public float yMargin = 0;
    public Color textColor = Color.white;
    float firstWordYPos;
    float firstWordXPos;
    int i = 0;
    public RectTransform previousWordRectTransform;
    Image bubbleBackImage;
    public RectTransform bubbleBackRectTransform;
    public RectTransform firstWordFromPreviousLine;

    bool jumpToNextLine = true;
    float targetYPos;

    Text textComponent;

    bool firstWordMustSpawn = true;

    public bool displayNewDialogueLine = false;
    public float bubbleMaximumWidth = 10f;

    public Canvas canvas;
    GameObject canvasObject;

    // Use this for initialization
    void Start ()
    {
        bubbleBackImage = gameObject.GetComponentInChildren<Image>();
        bubbleBackRectTransform = bubbleBackImage.gameObject.GetComponent<RectTransform>();
        textComponent = this.GetComponent<Text>();
        DisplayNewText(textToDisplay);

        canvasObject = GameObject.Find("GameStateManager");
        canvas = canvasObject.GetComponent<Canvas>();

    }

    GameObject SpawnWord ()
    {
        GameObject spawnedWord = Object.Instantiate(wordPrefab) as GameObject;
        Text newWordText = spawnedWord.GetComponent<Text>();
        newWordText.text += currentWordsToDisplay[i];
        newWordText.color = textColor;
        spawnedWord.transform.name = currentWordsToDisplay[i];
        newWordText.text += " ";
        return spawnedWord;
    }

    void setWordStartPos (bool isFirstWord, GameObject currentWord)
    {
        WordNextToFirstTest currentWordScript = currentWord.GetComponent<WordNextToFirstTest>();
        currentWordScript.parentDialogue = this;

        if (isFirstWord)
            currentWordScript.firstWord = true;
        else
        {
            currentWordScript.firstWord = false;
            currentWordScript.previousWord = previousWordRectTransform;
        }
    }

    public void ResetDialogueBubble ()
    {
        GameObject[] everySingleWord = GameObject.FindGameObjectsWithTag("DialogueWord");

        foreach (GameObject word in everySingleWord)
            GameObject.Destroy(word);

        firstWordMustSpawn = true;
        DisplayNewText(textToDisplay);
        i = 0;

        Debug.Log("Resetted dialogue box");
    }
	
	// Update is called once per frame
	void Update () {
        //We have to reset this component if a new line must be displayed
        if (displayNewDialogueLine)
        {
            ResetDialogueBubble();
            displayNewDialogueLine = false;
        }


       nextCharacterTimer += Time.deltaTime;
        if (nextCharacterTimer > displaySpeedRate && i < currentWordsToDisplay.Length)
        {
            GameObject spawnedNewWord = SpawnWord();
            setWordStartPos(firstWordMustSpawn, spawnedNewWord);
            firstWordMustSpawn = false;
            spawnedNewWord.transform.SetParent(this.transform);
            previousWordRectTransform = spawnedNewWord.GetComponent<RectTransform>();

            i++;
            nextCharacterTimer = 0;
        }
	}
    
    void DisplayNewText(string newText)
    {
        currentWordsToDisplay = textToDisplay.Split(' ');
    }
}
