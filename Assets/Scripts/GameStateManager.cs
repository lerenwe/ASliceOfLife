using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class GameStateManager : MonoBehaviour {

    public GameObject[] subScenes;
    public static GameObject currentActiveScene;
    public static GameObject player;
    public static Collider2D playerCollider;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();

        subScenes = GameObject.FindGameObjectsWithTag("subScene");
	}

    void Awake ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region Get Current Active Scene
        //The active scene is where the main character is
        Bounds localizedBackGroundBounds;
        SubSceneManager subSceneScript;

        //Get in which subScene the player is
        foreach (GameObject subScene in subScenes)
        {
            subSceneScript = subScene.GetComponent<SubSceneManager>();
            localizedBackGroundBounds = subSceneScript.myBackGround.sprite.bounds;
            localizedBackGroundBounds.center = subSceneScript.myBackGround.transform.position;

            if (localizedBackGroundBounds.Contains(player.transform.position))
                currentActiveScene = subScene;

            Debug.DrawLine(localizedBackGroundBounds.min, localizedBackGroundBounds.max, Color.red, 3f);
        }

        //Debug.Log("Current Active Scene = " + currentActiveScene);
        #endregion

    }
}
