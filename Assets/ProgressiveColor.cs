using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveColor : MonoBehaviour {


	string displayedText = null;
	List<string> currentWordsToDisplay = new List<string>();


	Text text;
	string colorString;
	Color color = Color.white;

	// Use this for initialization
	void Start () 
	{
		text = gameObject.GetComponent<Text> ();
		//currentWordsToDisplay = textToDisplay.Split(' ').ToList<String>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		color = Color.Lerp (color, new Color (1, 1, 1, 0f), 1f * Time.deltaTime);


			text.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
	}
}
