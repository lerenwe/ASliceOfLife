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
    int i = 0;
    RectTransform justSpawnedRectTransform;

    Text textComponent;

	// Use this for initialization
	void Start ()
    {
        textComponent = this.GetComponent<Text>();
        DisplayNewText(textToDisplay);
        justSpawnedRectTransform = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {

       nextCharacterTimer += Time.deltaTime;

        if (nextCharacterTimer > displaySpeedRate && i < currentWordsToDisplay.Length)
        {
            GameObject spawnedNewWord = Object.Instantiate(wordPrefab) as GameObject;

            Text newWordText = spawnedNewWord.GetComponent<Text>();
            newWordText.text += currentWordsToDisplay[i];
            spawnedNewWord.transform.name = currentWordsToDisplay[i];
            newWordText.text += " ";
            spawnedNewWord.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
            spawnedNewWord.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            spawnedNewWord.GetComponent<RectTransform>().position = justSpawnedRectTransform.position + new Vector3(justSpawnedRectTransform.rect.xMax + spawnedNewWord.GetComponent<RectTransform>().rect.size.x / 2, 0, 0);

            spawnedNewWord.transform.SetParent(mainCanvas.transform);

            i++;
            nextCharacterTimer = 0;
            justSpawnedRectTransform = spawnedNewWord.GetComponent<RectTransform>();
        }
	}
    
    void DisplayNewText(string newText)
    {
        //currentCharsToDisplay =  textToDisplay.ToCharArray();
        currentWordsToDisplay = textToDisplay.Split(' ');
    }
}
