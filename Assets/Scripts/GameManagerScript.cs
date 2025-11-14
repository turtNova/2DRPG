using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public int player1Hp = 100;
    public int player2Hp = 100;
    private GameObject player1;
    private GameObject player2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = GameObject.FindWithTag("player1");
        player2 = GameObject.FindWithTag("player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Hp == 0)
        {
            player1.SetActive(false);
        }
        if (player2Hp == 0)
        {
            player2.SetActive(false);
        }

    }

    public void DamagePlayer2(int damage)
    {
        player2Hp -= damage;
    }

    public void DamagePlayer1(int damage)
    {
        player1Hp -= damage;
    }
}
