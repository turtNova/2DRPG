using TMPro;
using UnityEngine;

public class StatUpdateScript : MonoBehaviour
{
    private PlayerMovementScript playerMovement;
    private TMP_Text text;

    private void Awake()
    {
        playerMovement = GameObject.FindWithTag("player1").GetComponent<PlayerMovementScript>(); ;
        text = GetComponent<TMP_Text>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = $"WASD: {playerMovement.moveDirection}";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"WASD: {playerMovement.moveDirection}\nJump: {playerMovement.onGround}";
    }
}
