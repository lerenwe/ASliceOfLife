using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    #region move variables
    Vector3 targetPosition = Vector3.zero;
    #endregion

    #region Components & Objects Variable
    public GameObject player;

    [SerializeField]
    public static SpriteRenderer currentBackGround;
    Camera camera;
    Rect CameraRect;
    #endregion

    // Use this for initialization
    void Start ()
    {
        #region setting up components & objects
        camera = gameObject.GetComponent<Camera>();
        CameraRect = camera.pixelRect;
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        //Get the current BackGround where the player is
        currentBackGround = GameStateManager.currentActiveScene.GetComponent<SubSceneManager>().myBackGround.GetComponent<SpriteRenderer>();

        targetPosition = new Vector3(player.transform.position.x, currentBackGround.transform.position.y, -10f);

        PreventGettingOutOfBackground();

        transform.position = targetPosition;
    }

    void PreventGettingOutOfBackground ()
    {
        Vector3 currentBackGroundLeftBorder = currentBackGround.transform.position - currentBackGround.sprite.bounds.extents;

        Vector3 cameraRightBorder = camera.ScreenToWorldPoint(new Vector3(CameraRect.width, CameraRect.height / 2, 0));
        Vector3 currentBackGroundRightBorder = currentBackGround.transform.position + currentBackGround.sprite.bounds.extents;

        float cameraHorizontalExtent = cameraRightBorder.x - transform.position.x;

        //Debug.DrawLine(camera.transform.position, new Vector3(camera.transform.position.x + cameraHorizontalExtent, 0, 0));

        if (targetPosition.x - cameraHorizontalExtent < currentBackGroundLeftBorder.x)
            targetPosition.x = currentBackGroundLeftBorder.x + cameraHorizontalExtent;
        else if (targetPosition.x + cameraHorizontalExtent > currentBackGroundRightBorder.x)
            targetPosition.x = currentBackGroundRightBorder.x - cameraHorizontalExtent;

    }
}
