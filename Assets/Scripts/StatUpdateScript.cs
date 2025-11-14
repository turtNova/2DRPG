using TMPro;
using UnityEngine;

public class StatUpdateScript : MonoBehaviour
{
    private Player1ActionScript playerMovement;
    private TMP_Text text;
    private GameManagerScript gameManagerScript;

    private void Awake()
    {
        playerMovement = GameObject.FindWithTag("player1").GetComponent<Player1ActionScript>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("gameManager").GetComponent<GameManagerScript>();
        text = GetComponent<TMP_Text>();
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"WASD: {playerMovement.moveDirection}\nP1 Jump: {playerMovement.onGround}\nP1 HP: {gameManagerScript.player1Hp}\nP2 HP: {gameManagerScript.player2Hp}";
    }
}
