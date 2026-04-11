using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int totalRound = 3;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    private int player1Wins = 0;
    private int player2Wins = 0;
    private int currentRound = 1;
    private int winsNeeded;
    
    public bool matchOver = false;

    void Awake()
    {
        Instance = this;
        winsNeeded = (totalRound / 2) + 1;
    }

    public void playerDied(string tag)
    {
        if(matchOver) return;

        if(tag == "Player1")
        {
            player2Wins++;
            Debug.Log(currentRound + ". Round winner is Player 2 !!!");
        }
        else if(tag == "Player2")
        {
            player1Wins++;
            Debug.Log(currentRound + ". Round winner is Player 1 !!!");
        }
        currentRound++;

        if(player2Wins >= winsNeeded)
        {
            Debug.Log("Player 2 Win Whe Match!");
            matchOver = true;
            Time.timeScale = 0;
            return;
        }
        
        if(player1Wins >= winsNeeded)
        {
            Debug.Log("Player 1 Win Whe Match!");
            matchOver = true;
            Time.timeScale = 0;
            Debug.Log(Time.timeScale);
            return;
        }

        player1.GetComponent<PlayerStats>().resetPlayer();
        player2.GetComponent<PlayerStats>().resetPlayer();

        
        
    }
}
