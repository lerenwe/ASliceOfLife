﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Ink.Runtime;

public class Dialogue : MonoBehaviour {

    #region Customizable variables
        [SerializeField]
        private TextAsset inkJSONAsset;

        public GameObject[] dialogueCharacters;
        public bool[] flipSideForThisCharacter;
    #endregion

    #region Dialogue Status
        [HideInInspector] public bool dialogueTriggered = false;
        [HideInInspector] public bool dialogueInProgress = false;
        [HideInInspector] public bool dialogueClosed = false;
        bool waitForInitFrame = false;

        int currentLineToDisplay = 0;
        bool dialogueStarted = false;
        bool lastLine = false;
        bool firstInit = true;
        bool waitingForChoice = false;
        bool displayChoiceNext = false;
    #endregion

    #region External objects & components
        Canvas thisCanvas;
        private Story story;
        DialogueDisplayer dialogueDisplayer;
        static public GameObject dialogueDisplayObject;
        [HideInInspector] public GameObject characterCurrentlySpeaking;
        string[] charactersName;
        RectTransform bubblePointRect;
        Image[] AllImages;
    #endregion
	
    //This is the method you want to call in order to start displaying the dialogue!
    public void TriggerDialogue ()
    {
        dialogueClosed = false; //Just in case the dialogue was previously closed
        dialogueDisplayObject.SetActive(true);
        dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();
        bubblePointRect = dialogueDisplayer.bubbleBackRectTransform.transform.FindChild("BubblePointer").GetComponent<RectTransform>();

        story = new Story(inkJSONAsset.text);

        charactersName = new string[dialogueCharacters.Length];
        int iterator = 0;
        foreach (GameObject character in dialogueCharacters)
        {
            charactersName[iterator] = character.transform.name;
            iterator++;
        }

        waitForInitFrame = true;
        dialogueTriggered = true;
        dialogueStarted = true;
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        if (firstInit && dialogueTriggered)
        {
            AllImages = dialogueDisplayer.AllImages;
            thisCanvas = dialogueDisplayer.canvas;
            firstInit = false;
        }

        if (!waitForInitFrame && !dialogueClosed)
        {
            if (dialogueTriggered || (dialogueInProgress && !lastLine && Input.GetButtonDown("Submit"))) //Display the first line & the next one when "Next Line" is pressed
            {
                Debug.Log("Waiting for choice is " + waitingForChoice);
                dialogueDisplayer.DisplayNewText(dialogueDisplayer.textToDisplay, waitingForChoice);
            }

            #region All of this stuff is to place the bubble correctly
            if (dialogueInProgress)
            {
                //The character's position we use is actually its upper sprite bound.
                Vector3 characterPosition = Camera.main.WorldToScreenPoint(characterCurrentlySpeaking.transform.position);
                Vector3 characterMaxBounds = Camera.main.WorldToScreenPoint(characterCurrentlySpeaking.GetComponent<SpriteRenderer>().bounds.max);

                Vector3 bubbleTargetPos;

                //This is to keep track of which character is speaking, and apply to each one wether the bubble must be displayed on their left or right
                int speakingCharacterNumber = 0;

                foreach (GameObject character in dialogueCharacters)
                {
                    if (character == characterCurrentlySpeaking)
                    {
                        break;
                    }
                    else
                    {
                        speakingCharacterNumber++;
                    }
                }

                //This is where we set up the position of the bubble according to the speaking character
                float characterWidth = Mathf.Abs(characterMaxBounds.x - characterPosition.x);

                if (!flipSideForThisCharacter[speakingCharacterNumber])
                {
                    bubbleTargetPos = new Vector3(characterPosition.x - dialogueDisplayer.bubbleBackRectTransform.rect.width / 2,
                        characterMaxBounds.y,
                        characterPosition.z);
                }
                else
                {
                    bubbleTargetPos = new Vector3(characterPosition.x + dialogueDisplayer.bubbleBackRectTransform.rect.width / 2,
                        characterMaxBounds.y,
                        characterPosition.z);
                }

                bubbleTargetPos.y += dialogueDisplayer.bubbleBackRectTransform.rect.size.y / 2 * thisCanvas.scaleFactor;

                //Then, let's prevent the bubble for getting out of the screen if the character is too close to one of the vertical borders of the screen
                float xMarginForBubble = Screen.width * dialogueDisplayer.screenMarginPercentage;

                bubbleTargetPos.x = Mathf.Clamp(bubbleTargetPos.x,
                                                xMarginForBubble + dialogueDisplayer.bubbleBackRectTransform.rect.size.x * thisCanvas.scaleFactor / 2,
                                                Screen.width - xMarginForBubble - dialogueDisplayer.bubbleBackRectTransform.rect.size.x * thisCanvas.scaleFactor / 2);

                dialogueDisplayer.bubbleBackRectTransform.position = bubbleTargetPos;

                //Let's do the same to prevent the pointer to get beyond the bubble's borders
                Vector3 bubblePointTargetPos = new Vector2(characterPosition.x, bubblePointRect.position.y);
                bubblePointTargetPos.x = Mathf.Clamp(bubblePointTargetPos.x,
                    dialogueDisplayer.bubbleBackRectTransform.position.x - dialogueDisplayer.bubbleBackRectTransform.rect.width * thisCanvas.scaleFactor / 2 + dialogueDisplayer.bubbleBackRectTransform.rect.width * .1f,
                    dialogueDisplayer.bubbleBackRectTransform.position.x + dialogueDisplayer.bubbleBackRectTransform.rect.width * thisCanvas.scaleFactor / 2 - dialogueDisplayer.bubbleBackRectTransform.rect.width * .1f);

                bubblePointRect.position = bubblePointTargetPos;

                //Then let's flip the pointer according to its position relative to the bubble's vertical center
                if (bubblePointRect.position.x < dialogueDisplayer.bubbleBackRectTransform.position.x)
                {
                    bubblePointRect.localScale = new Vector3(-0.1233374f, 0.1233374f, 0.1233374f);
                    bubblePointTargetPos.x += characterWidth + Screen.width * .01f;
                }
                else
                {
                    bubblePointRect.localScale = new Vector3(0.1233374f, 0.1233374f, 0.1233374f);
                    bubblePointTargetPos.x -= characterWidth - Screen.width * .01f;
                }

                bubblePointRect.position = bubblePointTargetPos;

                foreach (Image image in AllImages)
                {
                    image.enabled = true; // And finally display all images
                }
            }
            #endregion

            if (dialogueDisplayer != null)
            {
                //Skip line if it's being in the process of getting displayed and player pressed "Next Line" Button
                if (!dialogueClosed && Input.GetButtonDown("Submit") && !dialogueDisplayer.textScript.finished) 
                {
                    Debug.Log("Skipped line");
                    dialogueDisplayer.skipThisLine = true;
                }
                //But if the line has finished being displayed, 
                else if (dialogueTriggered || (dialogueInProgress && !lastLine &&Input.GetButtonDown("Submit") && dialogueDisplayer.textScript.finished)) 
                {
                    dialogueDisplayer.textScript.finished = false;
                    dialogueDisplayer.skipThisLine = false;
                    //Then we finally display the text on top of all that, except if we're inside a choice
                    string text = "Text Display Error. Check Dialogue.cs";

                    if (story.canContinue)
                    {
                        text = story.Continue().Trim();

                        //Parsing who's speaking right now
                        string firstWord = text.Split(':').First(); //TODO: Optimization : We can avoid this when we're displaying a choice.

                        foreach (GameObject character in dialogueCharacters)
                        {
                            if (firstWord.Equals(character.transform.name))
                            {
                                characterCurrentlySpeaking = character;
                                break;
                            }
                        }

                        if (characterCurrentlySpeaking == null)
                        {
                            Debug.LogError("None of the character's gameObject name matches the name ''" + firstWord + "'' in the Ink script. Please check that the names perfectly matches, including case.");
                        }

                        //Let's then remove the character name from the text to display
                        text = text.Replace(characterCurrentlySpeaking.transform.name + ":", "");
                        dialogueDisplayer.textToDisplay = text.Trim();

                        //At this point we mark the dialogue as in progress. Triggered is used only at the exact moment the dialogue is triggered.
                        dialogueDisplayer.ResetDialogueBubble(waitingForChoice);
                        dialogueTriggered = false;
                        dialogueInProgress = true;
                    }
                        



                    if (displayChoiceNext)
                    {
                        displayChoiceNext = false;
                        Debug.LogWarning("Reached a choice in the dialogue.");
                        DisplayChoices(ref text);
                    }
                    else
                    if (!story.canContinue && story.currentChoices.Count <= 0)
                    {
                        lastLine = true; //We reached the last line, the dialogue is now over and ready to be closed
                    }
                    else if (!story.canContinue && story.currentChoices.Count > 0)
                    {
                        displayChoiceNext = true;
                    } //TODO: Should reactivate this to work on the choice display



                    Debug.Log("NEXT LINE TO DISPLAY = " + text);
                }
                //Close dialogue when we reached the last line and the player pressed the "Next Line" key
                else if (dialogueStarted && lastLine &&Input.GetButtonDown("Submit") && dialogueDisplayer.finishedLine) 
                {
                    ResetDialogue();

                    dialogueClosed = true;

                    foreach (Image image in AllImages)
                    {
                        image.enabled = false;
                    }

                    /*GameObject[] AllWords = GameObject.FindGameObjectsWithTag("DialogueWord");

                    foreach (GameObject word in AllWords)
                    {
                        Destroy(word);
                    }*/

                    dialogueDisplayer.ResetDialogueBubble(false);

                    dialogueDisplayObject.SetActive(false);
                    //dialogueClosed = true;
                }
            }
        }

        waitForInitFrame = false;
    }

    void DisplayChoices (ref string text) //TODO : Cleanup : Not sure if the ref is needed after all...
    {
        text = charactersName[0].Trim() + ":";

        int iterator = 0;
        foreach (Ink.Runtime.Choice choice in story.currentChoices)
        {
            if (iterator != 0)
                text += "*";

            text += choice.text.Trim();

            iterator++;
        }

        dialogueDisplayer.textToDisplay = text.Trim();
        waitingForChoice = true;

        //TODO: IMPORTANT : From here, we can factorize all this stuff with the display of a regular line. Plz. Thx.
        //Parsing who's speaking right now
        string firstWord = text.Split(':').First(); //TODO: Optimization : We can avoid this when we're displaying a choice.

        foreach (GameObject character in dialogueCharacters)
        {
            if (firstWord.Equals(character.transform.name))
            {
                characterCurrentlySpeaking = character;
                break;
            }

            if (characterCurrentlySpeaking == null)
            {
                Debug.LogError("None of the character's gameObject name matches the name ''" + firstWord + "'' in the Ink script. Please check that the names perfectly matches, including case.");
            }
        }

        //Let's then remove the character name from the text to display
        text = text.Replace(characterCurrentlySpeaking.transform.name + ":", "");
        dialogueDisplayer.textToDisplay = text.Trim();

        //At this point we mark the dialogue as in progress. Triggered is used only at the exact moment the dialogue is triggered.
        dialogueDisplayer.ResetDialogueBubble(waitingForChoice);
        dialogueTriggered = false;
        dialogueInProgress = true;

        Debug.Log("Display choice called and completed");
    }

    void ResetDialogue() //Reset this dialogue when closed, in case it must be triggered again later
    {
        dialogueInProgress = false;
        dialogueTriggered = false;
        dialogueStarted = false;
        lastLine = false;
        firstInit = true;
        currentLineToDisplay = 0;
        story.ResetState();
    }
}
