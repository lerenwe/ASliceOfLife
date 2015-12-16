using UnityEngine;
using System.Collections;

public class subSceneExit : MonoBehaviour {

    public GameObject destinationPoint;

    [SerializeField]
    bool touchToExit;

    // Use this for initialization
    void Start () {
	
	}

    void Awake ()
    {
        #region check if mandatory components & objects are present
        if (destinationPoint == null)
            Debug.LogError(gameObject.name + " needs a destination point!");
        #endregion
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (touchToExit)
        {
            if (hit == GameStateManager.playerCollider)
                GameStateManager.player.transform.position = destinationPoint.transform.position;
        }
    }
}
