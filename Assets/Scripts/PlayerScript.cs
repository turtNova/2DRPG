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
    private Transform enemyTransform;
    public Vector2 moveDirection;
    public float moveSpeed = 10f;
    public float jumpForce = 20f;
    public float castDistance;
    public bool onGround = false;
    private LayerMask groundLayer; // Could be set to public and chosen in the editor instead, but I prefer this method
    private LayerMask playerLayer;
    private LayerMask enemyLayer;
    private int playerLayerInt;
    private int enemyLayerInt;

    // Damage & Health
    private GameManagerScript gameManagerScript;
    public GameObject attack1;
    public float attackCooldownTimer = 0f;
    public float attackCooldown = 0.3f;
    public float attackDuration = 0.1f;
    public float iFramesOnAttack;
    public float attackOffset;
    private float localIFrames = 0f;
    private Vector3 punchScale;
    private Vector3 playerScale;



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
        // Assign objects
        playerControls = new PlayerInputActions();
        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask($"player{3 - playerNumber}");
        enemyLayer = LayerMask.GetMask($"player{playerNumber}");

        // Math out variables
        playerLayerInt = 5 + playerNumber;
        enemyLayerInt = 8 - playerNumber;
        punchScale = attack1.transform.localScale;
        playerScale = transform.lossyScale;
    }

    void Start()
    {
        // Connect components
        playerRb = GetComponent<Rigidbody2D>();
        gameManagerScript = GameObject.FindWithTag("gameManager").GetComponent<GameManagerScript>();
        enemyTransform = GameObject.FindWithTag($"player{3 - playerNumber}").transform;
        castDistance = playerScale.y * gameObject.GetComponent<CircleCollider2D>().radius * 1.2f;
        attackOffset = punchScale.x / 2 + playerScale.x * gameObject.GetComponent<CircleCollider2D>().radius;
    }

    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        localIFrames -= Time.deltaTime;
        moveDirection = move.ReadValue<Vector2>();
        onGround = IsGrounded();
        if (jump.triggered && onGround)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        CheckAttack();
    }

    private void FixedUpdate()
    {
        // Sets the horizontal velocity to the horizontal movement axis
        playerRb.linearVelocityX = moveDirection.x * moveSpeed;
    }

    // Check if colliding with the Ground layer with a 2D Raycast pointed down
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
        // Spawns the attack hitbox as the child of the Player and offsets it based off the direction the enemy is in
        GameObject punch = Instantiate(attack1, transform.position + new Vector3(attackOffset * EnemyDirection(), 0f), transform.rotation, transform);
        punch.layer = enemyLayerInt;
        punch.transform.localScale = new Vector3(punchScale.x / playerScale.x, punchScale.y / playerScale.y, punchScale.z);
        punch.SetActive(true);
        Destroy(punch, attackDuration);
    }

    private int EnemyDirection()
    {
        if (gameObject.transform.position.x <= enemyTransform.position.x)
        { return 1; } else { return -1; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayerInt && localIFrames <= 0.01f)
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