using UnityEngine;
using System.Collections;

public class SubSceneManager : MonoBehaviour {

    public SpriteRenderer myBackGround;

	// Use this for initialization
	void Start () {
	
	}

    void Awake ()
    {
        #region check if mandatory components are missing
        if (myBackGround == null)
            Debug.LogError(gameObject.name + " sub-Scene manager need a background sprite!");
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
	}
}
