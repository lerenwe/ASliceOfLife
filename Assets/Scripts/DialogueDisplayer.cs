using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueDisplayer : MonoBehaviour {


    #region Display Internal Variables
        [HideInInspector] public string textToDisplay = "TEXT DISPLAY ERROR";
        [HideInInspector] public bool skipThisLine = false;

        string displayedText = null;
        List<string> currentWordsToDisplay = new List<string>();

        //Positioning & Line status
        [HideInInspector] public RectTransform previousWordRectTransform;
        [HideInInspector] public RectTransform bubbleBackRectTransform;
        [HideInInspector] public RectTransform firstWordFromPreviousLine;
        [HideInInspector] public Image bubbleBackImage;

        [HideInInspector] public bool m_bReachedLineEnd = false;
        [HideInInspector] public bool finishedLine = false;
        bool firstWordMustSpawn = true;
        bool wordWrapChoiceMode = false;
        bool displayOnNextFrame = false;

        float targetYPos;

		GameObject veryFirstWord;
		GameObject lastWordBeforeBreak;
		GameObject lastWord;
		Vector2 textBlockSize = Vector2.zero;
		bool finishedResizing = false;

        //The followings are used in case we are displaying a choice.
        Text textComponent;
        BubbleText textScript;
        bool recalculateBubbleSizeForChoices = false;
		Color color = Color.white;
    #endregion

    #region External Components
    //[Tooltip ("If you live this space blank, BubbleGum will automatically get the first active Canvas it can find.")]
        [HideInInspector]
        public Canvas canvas;
        GameObject NextLineLogo;
        [HideInInspector]
        public Image[] AllImages;
    #endregion

    #region Inspector Variables
        [Header("Required prefabs")]
        public GameObject wordPrefab;


        [Space(10)]
        [Header("Display customization")]
        public float displaySpeedRate = 1f;

        [Space(5)]
        public float xMargin = 0;
        public float yMargin = 0;

        [Space(5)]
        [Range(0, 1)]
        [Tooltip("Avoid the bubbles to get out of the screen, by adding a margin. Number represents percentage of the screen's X size.")]
        public float screenMarginPercentage = .1f;
        public float spaceBetweenLines = .2f;

        [Space(5)]
        public Color textColor = Color.white;
    #endregion

    #region Misc
        int iterator = 0;
        float nextCharacterTimer;
		public bool BrokeThisLineAtLeastOnce = false;
		GameObject previouslySpawnedWord = null;
    #endregion

    // Use this for initialization
    void Start ()
    {
        //Getting external components and objects
        canvas = gameObject.GetComponent<Canvas>();

        NextLineLogo = GameObject.Find("ContinueSpeechLogo").gameObject;
        bubbleBackImage = gameObject.GetComponentInChildren<Image>();
        bubbleBackRectTransform = bubbleBackImage.gameObject.GetComponent<RectTransform>();
        AllImages = transform.GetComponentsInChildren<Image>();
        textComponent = GameObject.Find("Text").GetComponent<Text>();
        textScript = GameObject.Find("Text").GetComponent<BubbleText>();

        //At game start, we want to hide the bubbles
        NextLineLogo.SetActive(false);
        
        foreach (Image image in AllImages)
        {
            image.enabled = false;
        }

        Dialogue.dialogueDisplayObject = this.gameObject; 
        //gameObject.SetActive(false);
    }

    //Set the word initial position right after being spawned
    //This also parents the word to the dialogueDisplayer, so we make sure it'll follow the bubble dynamically
    void setWordStartPos (bool isFirstWord, GameObject currentWord)
    {
        BubbleText currentWordScript = currentWord.GetComponent<BubbleText>();
        //currentWordScript.parentDialogue = this;

        if (isFirstWord) //We have to mark the word if it is the first one to spawn for this line, as its position will drive the rest of the words' positions.
        {
            if (wordWrapChoiceMode)
            {
                //currentWordScript.customWordWrap = false;
                currentWord.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap; //TODO : IMPORTANT : The text box should be resized for the bubble, or vice-versa, dunno, take a decision!
            }

            //currentWordScript.firstWord = true;
            //Debug.Log("''" + currentWordScript.name + "''" + " is the first word for this sentence");
        }
        else
        {
            if (wordWrapChoiceMode)
            {
                /*currentWordScript.customWordWrap = true;
                currentWordScript.choiceMode = true;*/
                currentWord.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap; //TODO : IMPORTANT : The text box should be resized for the bubble, or vice-versa, dunno, take a decision!
            }

            /*currentWordScript.firstWord = false;
            currentWordScript.previousWord = previousWordRectTransform; //This will be useful as every word that spawns after the first one will use the previously spawned word
                                                                        //position in order to place itself. */
        }


    }

    //Must be called each time we need to clear the bubble (Most of the time, it's to prepare the next line to be displayed).
    //This will also immediately start to display the next line of dialogue.
    public void ResetDialogueBubble (bool ChoiceMode)
    {
		BrokeThisLineAtLeastOnce = false;
        textScript.finished = false;
        textBlockSize = Vector2.zero;
		finishedResizing = false;

        firstWordMustSpawn = true;
        DisplayNewText(textToDisplay, ChoiceMode);
        iterator = 0;

        finishedLine = false;
        NextLineLogo.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {

        //This is used when the player press "Next Line" when the line is still being in the process of being displayed
        //TODO : This is not working with the actual display system, should rework it.
        /*if (skipThisLine && !finishedLine)
        {
            nextCharacterTimer = displaySpeedRate + 1;
        }
        else if (skipThisLine && finishedLine)
        {
            skipThisLine = false;
        }*/

        if (textScript.finished)
        {
            finishedLine = true;
            NextLineLogo.SetActive(true);
        }
	}

    private void LateUpdate()
    {
        if (displayOnNextFrame)
        {
            textComponent.text = textToDisplay;
            textScript.PlayNow();
            displayOnNextFrame = false;
        }
    }

    //This method actually separate each words of the line into a string array that will be used to spawn each word's gameObject individually
    public void DisplayNewText(string newText, bool ChoiceMode)
    {
        displayOnNextFrame = true;
    }

    }
