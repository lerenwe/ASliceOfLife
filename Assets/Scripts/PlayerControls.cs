using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    #region Controls Variables
    public float speedMultiplier = 5f;

    Vector3 moveDirection;

    bool canControl = true;
    #endregion

    #region My Components Variables
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    #endregion

    // Use this for initialization
    void Start ()
    {
        #region setting up my components
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        if (canControl)
        {
            Move();
        }

        ManageSprite();
        ManageAnimator();
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
