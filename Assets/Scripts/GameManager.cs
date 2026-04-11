using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int totalRound = 3;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI player1Score;
    [SerializeField] private TextMeshProUGUI player2Score;
    [SerializeField] public TextMeshProUGUI winner;

    public bool isRoundActive;
    private int player1Wins = 0;
    private int player2Wins = 0;
    private int currentRound = 1;
    private int winsNeeded;
    
    public bool matchOver = false;

    void Awake()
    {
        Instance = this;
        winsNeeded = (totalRound / 2) + 1;
        roundText.text = "Round " + currentRound;
        player1Score.text = "Player 1 Score : " + player1Wins;
        player2Score.text = "Player 2 Score : " + player2Wins;
    }

    public void playerDied(string tag)
    {
        StartCoroutine(RoundEnd(tag));
    }

    IEnumerator RoundEnd(string tag)
    {
        if(matchOver)  yield break;

        if(tag == "Player1")
        {
            player2Wins++;
            player2Score.text = "Player 2 Score : " + player2Wins;
            // Debug.Log(currentRound + ". Round winner is Player 2 !!!");
            winner.text = currentRound + ". Round winner is Player 2 !!!";
            StartCoroutine(winnerText());
        }
        else if(tag == "Player2")
        {
            player1Wins++;
            player1Score.text = "Player 1 Score : " + player1Wins;
            // Debug.Log(currentRound + ". Round winner is Player 1 !!!");
            winner.text = currentRound + ". Round winner is Player 1 !!!";
            StartCoroutine(winnerText());
        }
        currentRound++;

        if(player2Wins >= winsNeeded)
        {
            //Debug.Log("Player 2 Win Whe Match!");
            winner.text = "Player 2 Win Whe Match!";
            StartCoroutine(winnerText());
            matchOver = true;
            Time.timeScale = 0;
            yield break;
        }
        
        if(player1Wins >= winsNeeded)
        {
            //Debug.Log("Player 1 Win Whe Match!");
            winner.text = "Player 1 Win Whe Match!";
            StartCoroutine(winnerText());
            matchOver = true;
            Time.timeScale = 0;
            //Debug.Log(Time.timeScale);
            yield break;
        }

        DisablePlayer();

        yield return new WaitForSecondsRealtime(2f);

        player1.GetComponent<PlayerStats>().resetPlayer();
        player2.GetComponent<PlayerStats>().resetPlayer();
        roundText.text = roundText.text = "Round " + currentRound;
        EnablePlayer();
    }

    IEnumerator winnerText()
    {
        winner.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        winner.gameObject.SetActive(false);
    }

    void DisablePlayer()
    {
        player1.GetComponent<PlayerController>().isActive = false;
        player2.GetComponent<PlayerController>().isActive = false;
        player1.GetComponent<PlayerCombat>().isActive = false;
        player2.GetComponent<PlayerCombat>().isActive = false;
    }

    void EnablePlayer()
    {
        player1.GetComponent<PlayerController>().isActive = true;
        player2.GetComponent<PlayerController>().isActive = true;
        player1.GetComponent<PlayerCombat>().isActive = true;
        player2.GetComponent<PlayerCombat>().isActive = true;
    }
}
