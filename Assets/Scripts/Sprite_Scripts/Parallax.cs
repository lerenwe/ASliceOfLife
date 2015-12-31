﻿using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

    public GameObject mainBackGroundObject;
    SpriteRenderer mainBackGroundSprite;
    SpriteRenderer currentActiveBackGround;
    SpriteRenderer thisRenderer;

    public float offset = 1f;

	// Use this for initialization
	void Start ()
    {
        thisRenderer = this.GetComponent<SpriteRenderer>();

        if (mainBackGroundObject == null)
            Debug.LogError(gameObject.name + "parallax Script need to get main Background!");
        else
        {
            mainBackGroundSprite = mainBackGroundObject.GetComponent<SpriteRenderer>();
        }

        
    }

    void Awake ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentActiveBackGround = GameStateManager.currentActiveScene.GetComponent<SubSceneManager>().myBackGround.GetComponent<SpriteRenderer>();

        if (currentActiveBackGround == mainBackGroundSprite)
        {
            Vector3 mainBackGroundCenter = mainBackGroundObject.transform.position;

            float distBetweenBGandCamera = mainBackGroundObject.transform.position.x - Camera.main.transform.position.x;

            if (offset <= 1)
                transform.position = new Vector3 ( ((mainBackGroundObject.transform.position.x - distBetweenBGandCamera) * offset), transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(((mainBackGroundObject.transform.position.x + distBetweenBGandCamera) * offset), transform.position.y, transform.position.z);
        }

	}
}
