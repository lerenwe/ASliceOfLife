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
        public BubbleText textScript;
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
        ContentSizeFitter wordSizeFitter;
        
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
        wordSizeFitter = textComponent.GetComponent<ContentSizeFitter>();

        //At game start, we want to hide the bubbles
        NextLineLogo.SetActive(false);
        
        foreach (Image image in AllImages)
        {
            image.enabled = false;
        }

        Dialogue.dialogueDisplayObject = this.gameObject; 
        //gameObject.SetActive(false);
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
        if (skipThisLine && !textScript.finished)
        {
            textScript.play = false;
            textScript.progress.Clear();
            textScript.initialize = true;
            textScript.GetComponent<Text>().color = textScript.targetColorForText;
            textScript.finished = true;
            skipThisLine = false;
            textScript.GetComponent<Text>().SetAllDirty();
        }
        else if (skipThisLine && textScript.finished)
        {
            skipThisLine = false;
        }

        if (textScript.finished)
        {
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
        //textScript.finished = false;
        displayOnNextFrame = true;

        if (ChoiceMode)
        {
            List<String> allChoices = new List<string>();

            allChoices = textToDisplay.Split('*').ToList<String>();


            int iterator = 0;
            Vector3 previousChoicePos = Vector3.zero;
            foreach (String choice in allChoices)
            {
                if (iterator == 0)
                {
                    textToDisplay = choice;
                    //wordSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
                    wordSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize; //Text is now displayed correctly

                    //Now let's align the text rect with the upper side of the bubble rect
                    textComponent.transform.position = new Vector3(textComponent.transform.position.x,
                        (bubbleBackRectTransform.transform.position.y + (bubbleBackRectTransform.rect.size.y / 2) * canvas.scaleFactor) - (textComponent.rectTransform.rect.size.y) * canvas.scaleFactor, //TODO: This should work but there's a way to factorize it huh. Nah not working because I think it might use the text rect size from the previous frame....
                        textComponent.transform.position.z); 

                    previousChoicePos = textComponent.transform.position;
                    textComponent.SetAllDirty();
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    Vector3 targetPos = new Vector3(previousChoicePos.x , previousChoicePos.y - (textComponent.rectTransform.rect.size.y) * canvas.scaleFactor, previousChoicePos.z);

                    GameObject clonedChoice = GameObject.Instantiate(textComponent.gameObject);
                    clonedChoice.transform.SetParent(bubbleBackImage.transform);
                    clonedChoice.transform.localScale = Vector3.one;
                    clonedChoice.transform.position = targetPos;
                    clonedChoice.GetComponent<Text>().text = choice;
                    clonedChoice.GetComponent<BubbleText>().PlayNow();
                    previousChoicePos = clonedChoice.transform.position;
                }

                iterator++;
            }
        }
    }

    }
