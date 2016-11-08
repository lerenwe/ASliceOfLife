using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractionLogoPosition : MonoBehaviour {


    public bool enableUpdate = false;
    GameObject Player;
    SpriteRenderer playerSprite;
    Image image;

	// Use this for initialization
	void Start () {

        Player = GameObject.Find("Player");
        playerSprite = Player.GetComponent<SpriteRenderer>();

        image = gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
	    if (enableUpdate)
        {
            Vector3 characterPosition = Camera.main.WorldToScreenPoint(Player.transform.position);
            Vector3 characterMaxBounds = Camera.main.WorldToScreenPoint(playerSprite.GetComponent<SpriteRenderer>().bounds.max);
            image.rectTransform.position = new Vector3 (characterPosition.x, characterMaxBounds.y);
        }
	}
}
