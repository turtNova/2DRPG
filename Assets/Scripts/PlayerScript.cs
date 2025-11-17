using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerScript : MonoBehaviour
{
    // Player 1 or 2
    public int playerNumber = 1;

    // Input
    public PlayerInputActions playerControls;
    private InputAction move, jump, attack;

    // Physics
    private Rigidbody2D playerRb;
    public Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 10f;
    public float jumpForce = 20f;
    private float castDistance;
    public bool onGround = false;
    private LayerMask groundLayer; // Could be set to public and chosen in the editor instead, but I prefer this method

    // Damage & Health
    private GameManagerScript gameManagerScript;
    public GameObject attack1;
    public float attackCooldownTimer = 0f;
    public float attackCooldown = 0.3f;
    private float attackDuration = 20f;
    private float localIFrames = 0f;
    public float iFramesOnAttack = 0.5f;
    private int attackDirection;
    private LayerMask enemyLayer;


    private void OnEnable()
    {
        if (playerNumber == 1)
        {
            move = playerControls.Player1.Move;
            jump = playerControls.Player1.Jump;
            attack = playerControls.Player1.Attack;
        }
        else
        {
            move = playerControls.Player2.Move;
            jump = playerControls.Player2.Jump;
            attack = playerControls.Player2.Attack;
        }
        move.Enable();
        jump.Enable();
        attack.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        groundLayer = LayerMask.GetMask("Ground");
        enemyLayer = LayerMask.GetMask($"player{playerNumber}");
        attackDirection = 1 - 2 * (playerNumber - 1);
}
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        gameManagerScript = GameObject.FindWithTag("gameManager").GetComponent<GameManagerScript>();
        castDistance = transform.lossyScale.y * gameObject.GetComponent<CircleCollider2D>().radius * 1.2f;
        Debug.Log(castDistance);
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        localIFrames -= Time.deltaTime;
        moveDirection = move.ReadValue<Vector2>();
        //onGround = IsGrounded();
        if (jump.triggered && IsGrounded())
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //onGround = false;
        }
        CheckAttack();
    }

    private void FixedUpdate()
    {
        // Sets the horizontal velocity to the horizontal movement axis
        playerRb.linearVelocityX = moveDirection.x * moveSpeed;
    }

    // 2D Raycast pointed down method
    public bool IsGrounded()
    {
        // Spawns a raycast from the origin of the GameObject, which is better than offsetting the position and casting from the edge
        // This way prevents edge cases where collisions occur *inside* the gameobject
        // Raycast2D is an event based system
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance, groundLayer);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckAttack()
    {
        if (attack.triggered && attackCooldownTimer <= 0f)
        {
            attackCooldownTimer = attackDuration + attackCooldown;
            AttackSpawn();
        }
    }

    private void AttackSpawn()
    {
        Vector3 attackOffset = transform.position + new Vector3(-1f, 0f);
        GameObject punch = Instantiate(attack1, attackOffset, transform.rotation, transform);
        punch.layer = 7;
        punch.SetActive(true);
        Destroy(punch, attackDuration + attackCooldown);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 5 + playerNumber && localIFrames <= 0.01f)
        {
            localIFrames = iFramesOnAttack;
            Debug.Log($"Got hit by Player {playerNumber}");
            gameManagerScript.DamagePlayer(20, playerNumber);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * castDistance);
    }

    // Check if on ground with collisions
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //    {
    //        Debug.Log("Hit ground");
    //        onGround = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //    {
    //        onGround = false;
    //    }
    //}
}