using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    #region Controls Variables
    public float speedMultiplier = 5f;
    public float followDelayReaction = .5f;
    float timerForDelayReaction = 0;

    [HideInInspector]
    public Vector3 moveDirection;

    public bool moveTowardTargetPoint = false;
    public bool moveTowardPlayer = false;
    public bool onDestinationPoint = false;

    public GameObject targetPoint;

    GameObject player;
    [SerializeField]
    float towardPlayerOffset;
    public GameObject objectToLookAt;
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
        player = GameObject.Find("Player");
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        Move();

        ManageSprite();
        ManageAnimator();
    }

    void Move()
    {
        float distToTarget;

        if (moveTowardTargetPoint && targetPoint != null)
        {
            distToTarget = transform.position.x - targetPoint.transform.position.x;
            distToTarget = Mathf.Abs(distToTarget);

            if (distToTarget > .1f)
            {
                moveDirection = Vector3.Normalize(new Vector3(targetPoint.transform.position.x - transform.position.x, 0, 0));
                onDestinationPoint = false;
            }
            else
            {
                moveDirection = Vector3.zero;
                onDestinationPoint = true;
            }
        }

        if (moveTowardPlayer)
        {
            //Debug.Log(this.name + " walking toward Player.");
            distToTarget = transform.position.x - player.transform.position.x;
            distToTarget = Mathf.Abs(distToTarget);

            if (distToTarget > towardPlayerOffset)
            {
                timerForDelayReaction += Time.deltaTime;

                if (timerForDelayReaction > followDelayReaction)
                {
                    moveDirection = Vector3.Normalize(new Vector3(player.transform.position.x - transform.position.x, 0, 0));
                    onDestinationPoint = false;
                }
            }
            else
            {
                timerForDelayReaction = 0;
                moveDirection = Vector3.zero;
                onDestinationPoint = true;
            }
        }

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

    void ManageAnimator()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x * speedMultiplier));
    }
}
