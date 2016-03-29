using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

    public string textToDisplay = "lol";
    public GameObject wordPrefab;
    public GameObject mainCanvas;
    string displayedText = null;
    char[] currentCharsToDisplay;
    string[] currentWordsToDisplay;
    float nextCharacterTimer;
    public float displaySpeedRate = 1f;
    public float spaceBetweenLines = .2f;
    public Color textColor = Color.white;
    float firstWordYPos;
    float firstWordXPos;
    int i = 0;
    RectTransform justSpawnedRectTransform;
    Image bubbleBackImage;
    RectTransform bubbleBackRectTransform;

    bool jumpToNextLine = true;
    float targetYPos;

    Text textComponent;

	// Use this for initialization
	void Start ()
    {
        bubbleBackImage = gameObject.GetComponentInChildren<Image>();
        bubbleBackRectTransform = bubbleBackImage.GetComponent<RectTransform>();
        textComponent = this.GetComponent<Text>();
        DisplayNewText(textToDisplay);
        justSpawnedRectTransform = this.GetComponent<RectTransform>();
        targetYPos = justSpawnedRectTransform.position.y;
        firstWordYPos = justSpawnedRectTransform.position.y;
        firstWordXPos = justSpawnedRectTransform.position.x;
        //Bubble size
        
	}
	
	// Update is called once per frame
	void Update () {

       nextCharacterTimer += Time.deltaTime;

        //Let's check if the next word can be contained within the bubble


        if (nextCharacterTimer > displaySpeedRate && i < currentWordsToDisplay.Length)
        {
            GameObject spawnedNewWord = Object.Instantiate(wordPrefab) as GameObject;

            Text newWordText = spawnedNewWord.GetComponent<Text>();
            newWordText.text += currentWordsToDisplay[i];
            newWordText.color = textColor;
            spawnedNewWord.transform.name = currentWordsToDisplay[i];
            newWordText.text += " ";
            spawnedNewWord.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
            spawnedNewWord.GetComponent<ContentSizeFitter>().SetLayoutVertical();

            spawnedNewWord.GetComponent<RectTransform>().position = new Vector3 (justSpawnedRectTransform.position.x, targetYPos, 0)
            + new Vector3(justSpawnedRectTransform.rect.xMax 
            + spawnedNewWord.GetComponent<RectTransform>().rect.size.x / 2, 0, 0);

            //If new word go past bubble horizontal boundaries....
            float currentWordXBoundary = spawnedNewWord.GetComponent<RectTransform>().position.x + spawnedNewWord.GetComponent<RectTransform>().rect.size.x / 2;
            float currentBubbleXBoundary = bubbleBackRectTransform.position.x + bubbleBackRectTransform.rect.size.x / 2 ;

            if (currentWordXBoundary > currentBubbleXBoundary)
            {
                Debug.Log("CLEARLY, YOU HAVE CROSSED THE LINE, BITCH");
                targetYPos = firstWordYPos - justSpawnedRectTransform.rect.size.y / 2 - spaceBetweenLines;

                spawnedNewWord.GetComponent<RectTransform>().position = new Vector3(firstWordXPos, targetYPos, 0);

                firstWordXPos = spawnedNewWord.GetComponent<RectTransform>().position.x;
                firstWordYPos = spawnedNewWord.GetComponent<RectTransform>().position.y;
            }

            spawnedNewWord.transform.SetParent(mainCanvas.transform);

            i++;
            nextCharacterTimer = 0;

            justSpawnedRectTransform = spawnedNewWord.GetComponent<RectTransform>();

            if (jumpToNextLine)
            {
                targetYPos = justSpawnedRectTransform.position.y;
                jumpToNextLine = false;
            }
        }
	}
    
    void DisplayNewText(string newText)
    {
        //currentCharsToDisplay =  textToDisplay.ToCharArray();
        currentWordsToDisplay = textToDisplay.Split(' ');
    }
}
