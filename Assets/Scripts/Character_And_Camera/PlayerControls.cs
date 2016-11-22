using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    #region Controls Variables
    public float speedMultiplier = 5f;

    [HideInInspector]
    public Vector3 moveDirection;
    public static bool canControl = true;
    [HideInInspector]
    public bool isGrounded = false;

    public float gravity = 20f;
    #endregion

    #region My Components Variables
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    Animator animator;
    float distToGround;
    #endregion

    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask wallLayer;

    [HideInInspector]
    public bool shoveAnimFinished = false;
    InteractionLogoPosition interactLogo;

    // Use this for initialization
    void Start ()
    {
        #region setting up my components
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        #endregion

        distToGround = gameObject.GetComponent<Collider2D>().bounds.extents.y;

        interactLogo = GameObject.Find("InteractionLogo").GetComponent<InteractionLogoPosition>();
        interactLogo.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckIfGrounded();

        if (canControl)
        {
            Move();
        }
        else
            moveDirection = Vector3.zero;
    }

    void Update ()
    {
        Debug.DrawRay(transform.position, -Vector3.up * (distToGround + .1f));

        ManageSprite();
        ManageAnimator();
	}

    public IEnumerator isPlayerGrounded ()
    {
        while (!isGrounded)
        {
            Debug.Log("YUP IT'S GROUNDED");
            yield return true;
        }
    }

    void CheckIfGrounded ()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.3f, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - boxCollider.bounds.extents.x - .1f, transform.position.y), -Vector2.up, distToGround + 0.3f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + boxCollider.bounds.extents.x + .1f, transform.position.y), -Vector2.up, distToGround + 0.3f, groundLayer);

        RaycastHit2D[] groundTests = new RaycastHit2D[3];
        groundTests[0] = hit;
        groundTests[1] = hitLeft;
        groundTests[2] = hitRight;

        foreach (RaycastHit2D a_hit in groundTests)
        {
            if (a_hit.collider != null)
            {
                Debug.Log("RAYCAST GROUND HIT SOMETHING");
                if (a_hit.transform.CompareTag("Ground"))
                {
                    isGrounded = true;

                    Vector3 perpendicularMoveDir;
                    perpendicularMoveDir = new Vector2(-a_hit.normal.y, a_hit.normal.x) / Mathf.Sqrt((a_hit.normal.x * a_hit.normal.x) + (a_hit.normal.y * a_hit.normal.y));

                    if (Input.GetAxisRaw("Horizontal") < 0)
                        moveDirection = perpendicularMoveDir;
                    else if (Input.GetAxisRaw("Horizontal") > 0)
                        moveDirection = -perpendicularMoveDir;

                    Debug.DrawLine(moveDirection * 5, moveDirection * -5, Color.green);

                    /* Finding a perpendicular to the ground line...
                    v = P2 - P1
                    P3 = (-v.y, v.x) / Sqrt(v.x^2 + v.y^2) * h
                    P4 = (-v.y, v.x) / Sqrt(v.x^2 + v.y^2) * -h
                    */
                    break;
                }
                else
                {
                    isGrounded = false;
                }
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    void Move ()
    {
        moveDirection = (new Vector2(Input.GetAxisRaw("Horizontal"), 0)); //This lines makes sure that if there's no input, the player will stand still.



        if (!isGrounded)
            moveDirection = Vector2.zero;

            //Apply moves
            transform.Translate ( moveDirection * speedMultiplier * Time.deltaTime);

        Debug.DrawRay(transform.position, moveDirection * speedMultiplier, Color.red);
    }

    void ManageSprite()
    {
    //Flip sprite to face move direction
        if (moveDirection.x < 0)
            spriteRenderer.flipX = true;
        else if (moveDirection.x > 0)
            spriteRenderer.flipX = false;
    }

    void ManageAnimator ()
    {
        animator.SetFloat("outSpeed", Mathf.Abs (moveDirection.x));
    }

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (hit.CompareTag ("Exit") && !hit.GetComponent<subSceneExit>().touchToExit && hit.GetComponent<subSceneExit>().preRequisitesMatch)
        {
            Debug.Log("Overlapping Exit");
            interactLogo.gameObject.SetActive (true);
            interactLogo.enableUpdate = true;
        }
    }

    void OnTriggerExit2D (Collider2D hit)
    {
        if (hit.CompareTag("Exit"))
        {
            Debug.Log("Exited exit, well you know...");
            interactLogo.enableUpdate = false;
            interactLogo.gameObject.SetActive (false);
        }
    }
}
