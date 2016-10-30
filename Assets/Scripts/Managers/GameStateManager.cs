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
    public static bool InDialogue;

    public static Image FadeImage;

    bool TitleScreenSequence = true;
    bool TitleScreenTriggered = false;
    bool  TitleScreenFinished = false;

    public bool enablePushStart = false;

    Image titleLogo;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();

        FadeImage = GameObject.Find("BlackPic").GetComponent<Image>();

        subScenes = GameObject.FindGameObjectsWithTag("subScene");

        titleLogo = transform.FindChild("TitleLogo").GetComponent<Image>();
        StartCoroutine(FadeInAnyPicture(titleLogo));

        player.GetComponent<Animator>().SetTrigger("Sleep");
    }

    void Awake ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown && TitleScreenTriggered != true && titleLogo.color.a > .9f)
        {
            TitleScreenSequence = true;
            TitleScreenTriggered = true;
            Destroy(transform.Find("PressStart").gameObject);
        }

        if (TitleScreenSequence && TitleScreenTriggered)
        {
            StartCoroutine(FadeIn());
            TitleScreenSequence = false;
        }
        
        if (!TitleScreenSequence && TitleScreenTriggered && FadeImage.canvasRenderer.GetColor().a <= .01f && !TitleScreenFinished)
        {
            StartCoroutine(FadeOutAnyPicture(titleLogo));
            TitleScreenFinished = true;
        }



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

    public static IEnumerator FadeOutAnyPicture (Image pictureToFade)
    {
        pictureToFade.CrossFadeColor(new Color(0, 0, 0, 0), 1f, true, true);
        yield return new WaitForSeconds(1f);
    }

    public static IEnumerator FadeInAnyPicture(Image pictureToFade)
    {
        pictureToFade.CrossFadeColor(new Color(1, 1, 1, 1), 1f, true, true);
        yield return new WaitForSeconds(1f);
    }

    public static IEnumerator FadeIn ()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        //Debug.Log("Waiting for player to be grounded...");
        //Debug.Log("Player grounded = " + GameStateManager.PlayerControls.isGrounded);
        while (!GameStateManager.player.GetComponent<PlayerControls>().isGrounded)
            yield return null;
        //Debug.Log("Player is grounded, proceeding...");

        //Debug.Log("Fade In");
        FadingIn = true;
        FadeImage.CrossFadeColor(new Color(0, 0, 0, 0), 1f, false, true);
        PlayerControls.canControl = false;
        yield return new WaitForSeconds(1f);
        PlayerControls.canControl = true;
        FadingIn = false;
    }

    public static IEnumerator FadeOut()
    {
        Debug.Log("Fade Out");
        FadingOut = true;
        FadeImage.CrossFadeColor(new Color(0, 0, 0, 1), 1f, false, true);
        PlayerControls.canControl = false;
        yield return new WaitForSeconds(1f);
        PlayerControls.canControl = true;
        FadingOut = false;
    }

    void UpdateExits ()
    {
        
    }
}
