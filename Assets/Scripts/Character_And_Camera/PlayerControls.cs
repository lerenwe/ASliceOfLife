using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    #region Controls Variables
    public float speedMultiplier = 5f;

    [HideInInspector]
    public Vector3 moveDirection;
    [HideInInspector]
    public bool canControl = true;
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
    }

    // Update is called once per frame
    void Update ()
    {
        if (canControl)
        {
            Move();
        }
        else
            moveDirection = Vector3.zero;

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

    void Move ()
    {
        moveDirection = (new Vector2(Input.GetAxisRaw("Horizontal"), 0));

        //Let's use a RayCast to prevent the player bouncing against walls...
        // OVER HERE ! Bon alors, adapte le code pour qu'il fonctionne dans les deux sens, et finit le pour que quand un mur est detecté, on ne puisse plus bouger
        // le perso dans ce sens. Troudbal. Tu sais que tu codes pas si mal, toi ? <3
        Vector2 rayStart = new Vector2(transform.position.x + boxCollider.bounds.extents.x, transform.position.y - boxCollider.bounds.extents.y);
        Vector2 rayEnd = new Vector2(rayStart.x + .1f, rayStart.y + boxCollider.bounds.size.y);
        RaycastHit2D hitAWall = Physics2D.Linecast(rayStart, rayEnd, wallLayer);

        Debug.DrawLine(rayStart, rayEnd, Color.blue);

        if (hitAWall.collider != null)
            Debug.Log("HIT A WALL, OUUUUUCH");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.3f, groundLayer);

        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;

                Vector3 perpendicularMoveDir;
                perpendicularMoveDir = new Vector2(-hit.normal.y, hit.normal.x) / Mathf.Sqrt((hit.normal.x * hit.normal.x) + (hit.normal.y * hit.normal.y));

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
            }
            else
            {
                isGrounded = false;
                //gameObject.GetComponent<Rigidbody2D>().gravityScale = 30;
            }
        }
        else
        {
            isGrounded = false;
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 30;
        }

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
}
