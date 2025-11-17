using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StatUpdateScript : MonoBehaviour
{
    private PlayerScript player1;
    private TMP_Text text;
    private GameManagerScript gameManagerScript;

    private void Awake()
    {
        player1 = GameObject.FindWithTag("player1").GetComponent<PlayerScript>();
    }

    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("gameManager").GetComponent<GameManagerScript>();
        text = GetComponent<TMP_Text>();
        text.text = "";
    }

    void Update()
    {
        float p1CD = Mathf.Round(player1.attackCooldownTimer * 100);
        text.text = $"WASD: {player1.moveDirection}" +
            $"\nP1 Jump: {player1.onGround}" +
            $"\nP1 HP: {gameManagerScript.player1Hp}" +
            $"\nP2 HP: {gameManagerScript.player2Hp}" +
            $"\nP1 CD: {p1CD}";
    }
}
