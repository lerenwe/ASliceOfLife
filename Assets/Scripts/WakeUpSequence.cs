using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WakeUpSequence : MonoBehaviour {

    bool wokeUp = false;
    GameObject player;
    Image titleLogo;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        titleLogo = GameObject.Find("TitleLogo").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!wokeUp)
        {
            PlayerControls.canControl = false;

            if (titleLogo.canvasRenderer.GetColor().a <= .1f && Input.GetAxis ("Horizontal") != 0)
            {
                player.GetComponent<Animator>().SetTrigger("WakeUp");
            }

            if (titleLogo.canvasRenderer.GetColor().a <= .1f && (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")))
            {
                PlayerControls.canControl = true;
                wokeUp = true;
            }
        }
	}
}
