using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class GameStateManager : MonoBehaviour {

    public GameObject[] subScenes;
    public static GameObject currentActiveScene;
    public static GameObject player;
    public static Collider2D playerCollider;

    public static bool FadingIn;
    public static bool FadingOut;

    public static Image FadeImage;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        FadeImage = GameObject.Find("BlackPic").GetComponent<Image>();

        subScenes = GameObject.FindGameObjectsWithTag("subScene");

        StartCoroutine(FadeIn());
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

    public static IEnumerator FadeIn ()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        Debug.Log("Waiting for player to be grounded...");
        Debug.Log("Player grounded = " + GameStateManager.player.GetComponent<PlayerControls>().isGrounded);
        while (!GameStateManager.player.GetComponent<PlayerControls>().isGrounded)
            yield return null;
        Debug.Log("Player is grounded, proceeding...");

        Debug.Log("Fade In");
        FadingIn = true;
        FadeImage.CrossFadeColor(new Color(0, 0, 0, 0), 1f, false, true);
        player.GetComponent<PlayerControls>().canControl = false;
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerControls>().canControl = true;
        FadingIn = false;

        
    }

    public static IEnumerator FadeOut()
    {
        Debug.Log("Fade Out");
        FadingOut = true;
        FadeImage.CrossFadeColor(new Color(0, 0, 0, 1), 1f, false, true);
        player.GetComponent<PlayerControls>().canControl = false;
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerControls>().canControl = true;
        FadingOut = false;
    }

    void UpdateExits ()
    {
        
    }
}
