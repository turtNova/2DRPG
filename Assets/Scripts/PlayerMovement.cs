using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public PlayerInputActions playerControls;

    float moveSpeed = 5;
    float moveX, moveY;
    Vector2 moveDirection = Vector2.zero;

    private InputAction move;
    private InputAction jump;
    
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    void Start()
    {
        GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Multiplies the Y direction by 0 to remove moving up/down, since this is a 2D fighter
        playerRb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * 0);
        if moveDirection
    }
}
