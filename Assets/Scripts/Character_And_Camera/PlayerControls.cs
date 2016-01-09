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
    #endregion

    #region My Components Variables
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    float distToGround;
    #endregion

    [SerializeField]
    LayerMask groundLayer;
    public bool shoveAnimFinished = false;

    // Use this for initialization
    void Start ()
    {
        #region setting up my components
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f, groundLayer);

        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;
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

        //Apply moves
        rigidBody.velocity = moveDirection * speedMultiplier;
    }

    void ManageSprite()
    {
    //Flip sprite to face move direction
        if (rigidBody.velocity.x < 0)
            spriteRenderer.flipX = true;
        else if (rigidBody.velocity.x > 0)
            spriteRenderer.flipX = false;
    }

    void ManageAnimator ()
    {
        animator.SetFloat("outSpeed", Mathf.Abs (moveDirection.x));
    }
}
