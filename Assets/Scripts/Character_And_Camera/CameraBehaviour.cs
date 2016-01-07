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

        targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, -10f);

        PreventGettingOutOfBackground();

        transform.position = targetPosition;
    }

    void PreventGettingOutOfBackground ()
    {
        Vector3 currentBackGroundBottomLeftBorder = currentBackGround.transform.position - currentBackGround.sprite.bounds.extents;

        Vector3 cameraRightBorder = camera.ScreenToWorldPoint(new Vector3(CameraRect.width, CameraRect.height / 2, 0));
        Vector3 cameraTopBorder = camera.ScreenToWorldPoint(new Vector3(CameraRect.width / 2, CameraRect.height, 0));

        Debug.DrawLine(cameraRightBorder, cameraTopBorder, Color.blue);

        Vector3 currentBackGroundTopRightBorder = currentBackGround.transform.position + currentBackGround.sprite.bounds.extents;

        float cameraHorizontalExtent = cameraRightBorder.x - transform.position.x;

        

        float cameraVerticalExtent = cameraTopBorder.y - transform.position.y;


        if (targetPosition.x - cameraHorizontalExtent < currentBackGroundBottomLeftBorder.x)
            targetPosition.x = currentBackGroundBottomLeftBorder.x + cameraHorizontalExtent;
        else if (targetPosition.x + cameraHorizontalExtent > currentBackGroundTopRightBorder.x)
            targetPosition.x = currentBackGroundTopRightBorder.x - cameraHorizontalExtent;

        if (targetPosition.y + cameraVerticalExtent > currentBackGroundTopRightBorder.y)
            targetPosition.y = currentBackGroundTopRightBorder.y - cameraVerticalExtent;
        else if (targetPosition.y - cameraVerticalExtent < currentBackGroundBottomLeftBorder.y)
            targetPosition.y = currentBackGroundBottomLeftBorder.y + cameraVerticalExtent;

    }
}
