using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Ink.Runtime;

public class Dialogue : MonoBehaviour {

    [SerializeField]
    private TextAsset inkJSONAsset;
    private Story story;

    public Canvas thisCanvas;

    public bool dialogueTriggered = false;
    public bool dialogueInProgress = false;
    public string[] dialogueLines;
    public GameObject characterSpeaking;
    public GameObject[] dialogueCharacters;
    string[] charactersName;
    public bool dialogueClosed = false;
    public bool[] flipSideForThisCharacter;

    bool reachedDialogueEnd = false;
    bool dialogueStarted = false;
    bool lastLine = false;

    int currentLineToDisplay = 0;

    DialogueDisplayer dialogueDisplayer;
    public GameObject dialogueDisplayObject;

	// Use this for initialization
	void Start ()
    {
	}
	
    public void TriggerDialogue ()
    {
        story = new Story(inkJSONAsset.text);


        charactersName = new string[dialogueCharacters.Length];
        int iterator = 0;
        foreach (GameObject character in dialogueCharacters)
        {
            charactersName[iterator] = character.transform.name;
            iterator++;
        }

        dialogueTriggered = true;
        dialogueStarted = true;
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        if (!dialogueClosed)
        {
            if (dialogueTriggered || (dialogueInProgress && !lastLine && Input.GetKeyDown("space"))) //Display the first line
            {
                dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();
                dialogueDisplayObject.SetActive(true);

                dialogueDisplayer.DisplayNewText(dialogueDisplayer.textToDisplay);

                if (dialogueDisplayObject == null && dialogueDisplayObject.activeInHierarchy)
                    dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();
            }

            //All of this stuff is to place the bubble correctly
            if (dialogueInProgress)
            {
                //The character's position is actually its upper sprite bound.
                Vector3 characterPosition = Camera.main.WorldToScreenPoint(characterSpeaking.transform.position);
                Vector3 characterMaxBounds = Camera.main.WorldToScreenPoint(characterSpeaking.GetComponent<SpriteRenderer>().bounds.max);
                //Debug.Log("characterMaxBounds = " + characterMaxBounds);
                float characterHeight = Mathf.Abs (characterMaxBounds.y - characterPosition.y);
                float characterWidth = Mathf.Abs(characterMaxBounds.x - characterPosition.x);

                GameObject bubblePoint = dialogueDisplayer.bubbleBackRectTransform.transform.FindChild("BubblePointer").gameObject;
                RectTransform bubblePointRect = bubblePoint.GetComponent<RectTransform>();

                Vector3 bubbleTargetPos;

                int speakingCharacterNumber = 0;

                foreach (GameObject character in dialogueCharacters)
                {
                    if (character == characterSpeaking)
                    {
                        break;
                    }
                    else
                    {
                        speakingCharacterNumber++;
                    }
                }

                //This is where we set up the position of the bubble according to the speaking character
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
                bubbleTargetPos.x = Mathf.Clamp(bubbleTargetPos.x, xMarginForBubble + dialogueDisplayer.bubbleBackRectTransform.rect.size.x * thisCanvas.scaleFactor / 2, Screen.width - xMarginForBubble - dialogueDisplayer.bubbleBackRectTransform.rect.size.x * thisCanvas.scaleFactor / 2);
                dialogueDisplayer.bubbleBackRectTransform.position = bubbleTargetPos;
                //Debug.Log("Final Bubble Pos is " + dialogueDisplayer.bubbleBackRectTransform.position.x);

                //Let's do the same to prevent the pointer to get beyond the bubble's borders
                Vector3 bubblePointTargetPos = new Vector2(characterPosition.x, bubblePointRect.position.y);
                bubblePointTargetPos.x = Mathf.Clamp(bubblePointTargetPos.x,
                    dialogueDisplayer.bubbleBackRectTransform.position.x - dialogueDisplayer.bubbleBackRectTransform.rect.width * thisCanvas.scaleFactor / 2 + 50, //TO DO : REPLACE "50" BY PERCENTAGE OF BUBBLE'S WIDTH
                    dialogueDisplayer.bubbleBackRectTransform.position.x + dialogueDisplayer.bubbleBackRectTransform.rect.width * thisCanvas.scaleFactor / 2 - 50);
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
            }

            if (dialogueDisplayer != null)
            {
                if (!dialogueClosed && Input.GetKeyDown("space") && !dialogueDisplayer.finishedLine) //Skip line
                {
                    Debug.Log("Skipped dialogue line");
                    dialogueDisplayer.skipThisLine = true;
                }
                else if (dialogueTriggered || (dialogueInProgress && !lastLine && Input.GetKeyDown("space") && dialogueDisplayer.finishedLine)) //Display the next line
                {
                    //Then we finally display the text on top of all that
                    string text = story.Continue().Trim(); // EXPERIMENTAL INK

                    //Parsing who's speaking right now
                    string firstWord = text.Split(':').First();

                    foreach (GameObject character in dialogueCharacters)
                    {
                        if (firstWord.Equals(character.transform.name))
                        {
                            characterSpeaking = character;
                            break;
                        }
                    }

                    //dialogueDisplayer.textToDisplay = dialogueLines[currentLineToDisplay];
                    text = text.Replace(characterSpeaking.transform.name + ":", "");
                    dialogueDisplayer.textToDisplay = text.Trim(); // EXPERIMENTAL INK

                    dialogueDisplayer.ResetDialogueBubble();
                    dialogueTriggered = false;
                    dialogueInProgress = true;


                    /*if (currentLineToDisplay < dialogueLines.Length - 1)
                        currentLineToDisplay++;
                    else
                    {
                        dialogueInProgress = false;
                        Debug.Log("Dialogue is over");
                    }*/

                    if(!story.canContinue)
                    {
                        lastLine = true;
                        Debug.Log("Dialogue is over");
                    }
                }
                else if (dialogueStarted && lastLine && Input.GetKeyDown("space") && dialogueDisplayer.finishedLine) //Close dialogue 
                {
                    Debug.Log("End of Dialogue");
                    dialogueInProgress = false;
                    dialogueDisplayObject.SetActive(false);
                    dialogueClosed = true;
                }
            }

        }
    }
}
