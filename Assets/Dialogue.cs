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
    public float xMargin = 0;
    public float yMargin = 0;
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

    bool firstWordMustSpawn = true;

	// Use this for initialization
	void Start ()
    {
        bubbleBackImage = gameObject.GetComponentInChildren<Image>();
        bubbleBackRectTransform = bubbleBackImage.gameObject.GetComponent<RectTransform>();
        textComponent = this.GetComponent<Text>();
        DisplayNewText(textToDisplay);
            
	}
	
	// Update is called once per frame
	void Update () {

       nextCharacterTimer += Time.deltaTime;

        //Let's check if the next word can be contained within the bubble


        if (nextCharacterTimer > displaySpeedRate && i < currentWordsToDisplay.Length)
        {

            GameObject spawnedNewWord = Object.Instantiate(wordPrefab) as GameObject;

            if(!firstWordMustSpawn)
                spawnedNewWord.GetComponent<WordNextToFirstTest>().previousWord = justSpawnedRectTransform;


            Text newWordText = spawnedNewWord.GetComponent<Text>();
            newWordText.text += currentWordsToDisplay[i];
            newWordText.color = textColor;
            spawnedNewWord.transform.name = currentWordsToDisplay[i];
            newWordText.text += " ";

            //spawnedNewWord.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
            //spawnedNewWord.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            //newWordText.resizeTextForBestFit = true;

            if (firstWordMustSpawn)
            {
                justSpawnedRectTransform = spawnedNewWord.GetComponent<RectTransform>();

                Debug.Log("JustSpawnedRectSize is " + justSpawnedRectTransform.rect.size.x);

                firstWordXPos = bubbleBackRectTransform.position.x /*+ xMargin*/;
                firstWordYPos = bubbleBackRectTransform.position.y /*- yMargin*/;
                spawnedNewWord.GetComponent<RectTransform>().position = new Vector2(firstWordXPos, firstWordYPos);

                targetYPos = justSpawnedRectTransform.position.y;
            }

                spawnedNewWord.GetComponent<WordNextToFirstTest>().targetYPos = targetYPos;

            if (!firstWordMustSpawn)
            {
                spawnedNewWord.GetComponent<RectTransform>().position = new Vector3(justSpawnedRectTransform.position.x, targetYPos, 0)
                + new Vector3(justSpawnedRectTransform.rect.xMax
                + spawnedNewWord.GetComponent<RectTransform>().rect.size.x / 2, 0, 0);
            }

            //If new word go past bubble horizontal boundaries....
            if (!firstWordMustSpawn)
            {
                float currentWordXBoundary = spawnedNewWord.GetComponent<RectTransform>().position.x + spawnedNewWord.GetComponent<RectTransform>().rect.size.x / 2;
                float currentBubbleXBoundary = bubbleBackRectTransform.position.x + bubbleBackRectTransform.rect.size.x / 2;


                if (currentWordXBoundary > currentBubbleXBoundary && !firstWordMustSpawn)
                {
                    Debug.Log("CLEARLY, YOU HAVE CROSSED THE LINE, BITCH");
                    targetYPos = firstWordYPos - justSpawnedRectTransform.rect.size.y / 2 - spaceBetweenLines;

                    spawnedNewWord.GetComponent<RectTransform>().position = new Vector3(firstWordXPos, targetYPos, 0);

                    firstWordXPos = spawnedNewWord.GetComponent<RectTransform>().position.x;
                    firstWordYPos = spawnedNewWord.GetComponent<RectTransform>().position.y;
                }
            }


            spawnedNewWord.transform.SetParent(this.transform);

            i++;
            nextCharacterTimer = 0;

            justSpawnedRectTransform = spawnedNewWord.GetComponent<RectTransform>();

            if (jumpToNextLine)
            {
                targetYPos = justSpawnedRectTransform.position.y;
                jumpToNextLine = false;
            }

            firstWordMustSpawn = false;
        }
	}
    
    void DisplayNewText(string newText)
    {
        //currentCharsToDisplay =  textToDisplay.ToCharArray();
        currentWordsToDisplay = textToDisplay.Split(' ');
    }
}
