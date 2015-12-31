using UnityEngine;
using System.Collections;

public class FootBall : MonoBehaviour {

    GameObject Player;
    PlayerControls playerScript;

    bool alreadyLaunched = false;

    // Use this for initialization
    void Start ()
    {
        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<PlayerControls>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (SoccerSequenceManager.soccerSequenceMainScript.sequenceStartTriggered && !alreadyLaunched)
        {
            transform.GetComponent<Rigidbody2D>().WakeUp();
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(-1, .2f, 0) * 250);
            transform.GetComponent<Rigidbody2D>().AddTorque (100f);
            alreadyLaunched = true;
        }

        if (SoccerSequenceManager.soccerSequenceMainScript.ballShot && !this.gameObject.GetComponent<Renderer>().isVisible)
            SoccerSequenceManager.soccerSequenceMainScript.ballDisappeared = true;

        if (SoccerSequenceManager.soccerSequenceMainScript.ballDisappeared)
            Destroy(this.gameObject);
	}

    void OnCollisionEnter2D (Collision2D hit)
    {
        if (hit.collider.gameObject == Player)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            if (playerScript.moveDirection != Vector3.zero)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(2, 1, 0) * 250);
                SoccerSequenceManager.soccerSequenceMainScript.ballShot = true;
            }
        }
    }
}
