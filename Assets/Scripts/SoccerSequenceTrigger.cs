using UnityEngine;
using System.Collections;

public class SoccerSequenceTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (hit.gameObject.CompareTag ("Player"))
        {
            SoccerSequenceManager.soccerSequenceMainScript.sequenceStartTriggered = true;
            Debug.Log("Starting soccer sequence...");
        }
    }
}
